using System.Collections.Generic;
using UnityEngine;
using SpaceOrigin.Data;

namespace SpaceOrigin.MahjongConnect
{
    public class GameManager : MonoBehaviour
    {
        public IntSO selectedLevelIndexSO; // this sets from the title menu
        public LevelManager levelManager;
        public LevelSessionRenderer levelSessionRender;
        public Transform worldCenterTransform;
        public AudioClip spriteSelectSfx;
        public AudioClip spriteDestroySfx;
        public AudioSource audioSource;

        private SpriteObject _currentSelectedObject;
        private LevelSession _currentLevelSession;
        private float _lastHintTime;
        private float _hintShowInterval = 10.0f; // hint will be shows in this interval, automode
        private bool _gameRunning;
        private const int _connectSuccesScore = 15;
        private const int _connectFailScore = 10;

        public delegate void GameScoreDelegate(int score);
        public GameScoreDelegate ongameScoreUpdate;

        public delegate void GameStateDelegate();
        public GameStateDelegate onGameWin;
        public GameStateDelegate onGameLost;

        public bool GameRunning { get => _gameRunning; set => _gameRunning = value; }

        private void Start()
        {
            _lastHintTime = Time.time;// just refernce time for counter
            LoadGame(selectedLevelIndexSO.Value);
        }

        private void Update()
        {
            if (!_gameRunning) return;

            if (_lastHintTime + _hintShowInterval <= Time.time)
            {
                List<SpriteObject> hintObjects = _currentLevelSession.GetHintSpriteObjects();
                _lastHintTime = Time.time;

                if (hintObjects == null)
                {
                    // not hint meants game over
                    _gameRunning = false;
                    LevelFaild();
                    return;
                }

                for (int i = 0; i < hintObjects.Count; i++)
                {
                    hintObjects[i].EnableHint();
                }
            }
        }

        private void LoadGame(int levelIndex)
        {
            _currentLevelSession = levelManager.GetLevelSession(levelIndex, worldCenterTransform.position);
            levelSessionRender.RenderSessionOnScreen(_currentLevelSession);
            GameRunning = true;
        }

        public void SelectObject(SpriteObject newSelectedObj, Color selColor, Color unSelColor)
        {
            if (!GameRunning) return;

            if (_currentSelectedObject != null)
            {
                if ( _currentSelectedObject == newSelectedObj) // clicked on same sprite, unselsect and return
                {
                    _currentSelectedObject.UnSelect(unSelColor);
                    _currentSelectedObject = null;
                    audioSource.clip = spriteSelectSfx;
                    audioSource.Play();
                    return;
                }

                if (_currentSelectedObject.SpriteType == newSelectedObj.SpriteType) // both are same types
                {
                    bool statusA = _currentLevelSession.CheckIfNearBy(_currentSelectedObject, newSelectedObj); // check again close ones
                    bool statusB = false; // we only need to check if they are not near by
                    if (!statusA) statusB  = CheckValidConnection(newSelectedObj, _currentSelectedObject);  // check against far ones

                    if (statusA || statusB)
                    {
                        _currentLevelSession.RemoveCellFromConnectionMap(_currentSelectedObject); // removing cell from connection map
                        _currentSelectedObject.Destroy(unSelColor);
                        _currentLevelSession.RemoveCellFromConnectionMap(newSelectedObj);
                        newSelectedObj.Destroy(unSelColor);
                        _currentSelectedObject = null;
                        _lastHintTime = Time.time;
                        AddScore();
                        audioSource.clip = spriteDestroySfx;
                        audioSource.Play();
                        _currentLevelSession.RemainigSpritObjCnt -=2 ;
                        if (_currentLevelSession.RemainigSpritObjCnt == 0) LevelCompleted();

                        return;
                    }
                    else ReduceScore();
                }
                else
                {
                    ReduceScore();
                }
            }
               
            SpriteObject oldObkject = _currentSelectedObject;
            _currentSelectedObject = newSelectedObj;
            _currentSelectedObject.SelectObject(selColor);
            oldObkject?.UnSelect(unSelColor);

            audioSource.clip = spriteSelectSfx;
            audioSource.Play();
        }

        private bool CheckValidConnection(SpriteObject startObject, SpriteObject endObject)
        {
            // will try to move to all directions
            bool statusUp = _currentLevelSession.CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, _currentLevelSession.UpDirection, 0);
            bool statusDown = _currentLevelSession.CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, _currentLevelSession.DownDirection, 0);
            bool statusRight = _currentLevelSession.CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, _currentLevelSession.RightDirection, 0);
            bool statusLeft = _currentLevelSession.CalculateValidTurns(startObject.ParentCell.CellPositionIndex, endObject.ParentCell.CellPositionIndex, _currentLevelSession.LeftDirection, 0);
            return (statusUp || statusDown || statusRight || statusLeft);
        }

        private void AddScore()
        {
            int gameScore = _currentLevelSession.SessionScore;
            gameScore += _connectSuccesScore;
            _currentLevelSession.SessionScore = gameScore;
            ongameScoreUpdate?.Invoke(gameScore);
        }

        private void ReduceScore()
        {
            int gameScore = _currentLevelSession.SessionScore;
            gameScore -= _connectFailScore;
            gameScore = gameScore < 0 ? 0 : gameScore;
            _currentLevelSession.SessionScore = gameScore;
            ongameScoreUpdate?.Invoke(gameScore);
        }

        private void LevelCompleted() // called when player clears all sprites
        {
            _gameRunning = false;
            SaveLevelStatus(true);
            onGameWin?.Invoke();
        }

        private void LevelFaild() // called when there is anymore moves left
        {
            _gameRunning = false;
            onGameLost?.Invoke();
        }

        private void SaveLevelStatus(bool completedLevel) // saves the current level status persistant
        {
            int higscore = 0;
            LevelStatus levelStatus = LevelHistory.GetLevelHistory(selectedLevelIndexSO.Value);
            if (levelStatus != null)
            {
                higscore = levelStatus.highScore;
            }
            else
            {
                levelStatus = new LevelStatus(); 
            }

            higscore = _currentLevelSession.SessionScore > higscore ? _currentLevelSession.SessionScore : higscore;
            levelStatus.completed = completedLevel;
            levelStatus.levelIndex = selectedLevelIndexSO.Value;
            levelStatus.highScore = higscore;
            levelStatus.currentScore = _currentLevelSession.SessionScore;
            LevelHistory.SaveLevelStatus(levelStatus);
        }
    }
}

