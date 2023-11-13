using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace BoardManager
{
    public class BoardManager : MonoBehaviour
    {
        private GameObject _clickedObject;


        [SerializeField] private GameObject xSign;
        [SerializeField] private GameObject oSign;
        [SerializeField] private GameObject _xWinText;
        [SerializeField] private GameObject _oWinText;
        
        private string[,] _board;

        private int _boardTileCount = 3;

        private bool _XFirst;
        private bool _xPlayerTurn;
        private bool _xWin;
        private bool _oWin;

        private void Start()
        {
            _board = new string[_boardTileCount, _boardTileCount];
            _xWin = false;
            _oWin = false;
            
            _XFirst = FirstTurn(_XFirst);
            Debug.Log(_XFirst ? $"X first turn. XFirst = {_XFirst}" : $"O first turn. XFirst = {_XFirst}");
            
            _xPlayerTurn = _XFirst;
        }

        private void Update()
        {
            if (_xWin == false && _oWin == false)
            {
                PlayerVPlayer();
                FindWinner(_board);
            }

            if (_xWin == true)
            {
                _xWinText.SetActive(true);
            } 
            else if (_oWin == true) 
            {
                _oWinText.SetActive(true);
            }
        }


        private bool FirstTurn(bool XFirstTurn)
        {
            XFirstTurn = true;
            float random = Random.Range(0, 1.0f);
            
            if (random >= 0.5f)
            {
                XFirstTurn = false;
            }
            
            Debug.Log("Random result is " + random);
            return XFirstTurn;
        }

        private GameObject GetClickedObject(GameObject hitObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    hitObject = hit.collider.gameObject;
                }
                Debug.Log($"Clicked object is {hitObject}");
            }
            return hitObject;
        }

        private void PutSignOnTile(GameObject someSign, GameObject clickedObject)
        {
            bool isTileClicked = IsTileClicked(_clickedObject);
            
            if (isTileClicked)
            {
                GameObject newObject = Instantiate(someSign);
                newObject.transform.parent = clickedObject.transform;
                
                float yAccomodation = 0.52f;
                Vector3 newPosition = clickedObject.transform.position;

                newObject.transform.position = new Vector3(x: newPosition.x, y: newPosition.y + yAccomodation, newPosition.z);
                Debug.Log("Put sign here: " + clickedObject.name);
            }
            else
            {
                Debug.Log("It is not a Tile or the Tile contains X/O sign.");
            }
        }

        private void PlayerVPlayer() 
        {
            if (Input.GetMouseButtonDown(0))
            {
                _clickedObject = GetClickedObject(_clickedObject);
                bool isTileClicked = IsTileClicked(_clickedObject);

                switch (_xPlayerTurn)
                {
                    case true:
                        Debug.Log($"xPlayer = {_xPlayerTurn}");
                        
                        if (isTileClicked)
                        {
                            PutSignOnTile(xSign, _clickedObject);
                            Debug.Log("X turn ended.");
                            WriteSignInBoard(xSign.name, oSign.name, _clickedObject, _xPlayerTurn);
                            _xPlayerTurn = !_xPlayerTurn;
                        }
                        break;
                    
                    case false:
                        Debug.Log($"xPlayer = {_xPlayerTurn}");
                        
                        if (isTileClicked)
                        {
                            PutSignOnTile(oSign, _clickedObject);
                            Debug.Log("O turn ended.");
                            WriteSignInBoard(xSign.name, oSign.name, _clickedObject, _xPlayerTurn);
                            _xPlayerTurn = !_xPlayerTurn;
                        }
                        break;
                }
            }
        }

        private bool IsTileClicked(GameObject clickedObject)
        {
            bool isTileClicked = false;
            GameObject tileChild = GetChildGameObjectWithTag(clickedObject, xSign.tag);

            if (clickedObject.CompareTag("Tile") && tileChild == null)
            {
                isTileClicked = true;
            }
            else
            {
                Debug.Log("Click on tile.");
            }
            
            return isTileClicked;
        }
        
        static public GameObject GetChildGameObjectWithTag(GameObject fromGameObject, string childTag) 
        {
            if (fromGameObject != null)
            {
                Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
            
                foreach (Transform t in ts)
                    if (t.gameObject.CompareTag(childTag)) return t.gameObject;
            }
            return null;
        }

        private string[,] WriteSignInBoard(string xName, string oName, GameObject clickedObject, bool xPlayerTurn)
        {
            int line = GetTileIndex(clickedObject.name, 0);
            int column = GetTileIndex(clickedObject.name, 2);

            if (xPlayerTurn)
            {
                _board[line, column] = xName;
                Debug.Log($"Wrote the {_board[line, column]} into the line: {line}; column: {column}.");
            } 
            else
            {
                _board[line, column] = oName;
                Debug.Log($"Wrote the {_board[line, column]} into the line: {line}; column: {column}.");
            }
            return _board;
        }

        private int GetTileIndex(string clickedObjectName, int arrayIndex)
        {
            char[] array = clickedObjectName.ToCharArray();
            int index;

            index = int.Parse(array[arrayIndex].ToString());
            
            return index;
        }

        private bool FindWinner(string[,] board)
        {
            for (int i = 0; i < _boardTileCount; i++)
            {
                if (board[0, i] != null && board[1, i] != null && board[0, i] != null)
                {
                    if (board[0, i] == board[1, i] && board[0, i] == board[2, i])
                    {
                        if (board[0, i] == xSign.name)
                        {
                            _xWin = true;
                            Debug.Log("xWin");
                            return _xWin;
                        }
                        else if (board[0, i] == oSign.name)
                        {
                            _oWin = true;
                            Debug.Log("oWin");
                            return _oWin;
                        }
                    }                
                }
            }

            for (int i = 0; i < _boardTileCount; i++)
            {
                if (board[i, 0] != null && board[i, 1] != null && board[i, 2] != null)
                {
                    if (board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2])
                    {
                        if (board[i, 0] == xSign.name)
                        {
                            _xWin = true;
                            Debug.Log("xWin");
                            return _xWin;
                        }
                        else if (board[i, 0] == oSign.name)
                        {
                            _oWin = true;
                            Debug.Log("oWin");
                            return _oWin;
                        }
                    }
                }
            }

            if (board[0, 0] != null && board[1, 1] != null && board[2, 2] != null)
            {
                if (board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2])
                {
                    if (board[0, 0] == xSign.name)
                    {
                        _xWin = true;
                        Debug.Log("xWin");
                        return _xWin;
                    }
                    else if (board[0, 0] == oSign.name)
                    {
                        _oWin = true;
                        Debug.Log("oWin");
                        return _oWin;
                    }
                }
            }

            if (board[0, 2] != null && board[1, 1] != null && board[2, 0] != null)
            {
                if (board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0])
                {
                    if (board[0, 2] == xSign.name)
                    {
                        _xWin = true;
                        Debug.Log("xWin");
                        return _xWin;
                    }
                    else if (board[0, 2] == oSign.name)
                    {
                        _oWin = true;
                        Debug.Log("oWin");
                        return _oWin;
                    }
                }
            }

            return false;
        }
    }
}