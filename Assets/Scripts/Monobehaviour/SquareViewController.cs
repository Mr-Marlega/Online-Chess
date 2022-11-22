using ChessHelper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareViewController : MonoBehaviour
{
    [SerializeField]
    RawImage bgImage;
    [SerializeField]
    Image TroopImage;
    Button SquareButton;
    Square mSquare;

    private void OnEnable()
    {
        if(!SquareButton)
            SquareButton = bgImage.GetComponent<Button>();
        SquareButton.onClick.AddListener(OnChessSquareClicked);
    }
    public void SetImageSize(float dim)
    {
        bgImage.rectTransform.sizeDelta = new Vector2(dim, dim);
        TroopImage.rectTransform.sizeDelta = new Vector2(dim, dim);
    }
    public void SetTroopImageRotation(COLOR _color)
    {
        if(_color == COLOR.BLACK)
        {
            TroopImage.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
        }
        if(_color == COLOR.WHITE)
        {
            TroopImage.transform.localRotation = Quaternion.identity;
        }
    }
    public void setSquarePos(float rowPos, float colPos)
    {
        transform.localPosition = new Vector3(rowPos, colPos, 0);
    }
    public void SetSquare(Square _square)
    {
        mSquare = _square;
        mSquare.onStateChange += OnSquareStateChanged;
        mSquare.onTroopChange += OnTroopChanged;
        if (_square.Troop.GetPiece() == PIECE.NONE)
        {
            TroopImage.gameObject.SetActive(false);
        } 
        else
        {
            TroopImage.sprite = GameManager.instance.assetManager.
                GetTroopImage(_square.Troop.GetPiece(), _square.Troop.GetPieceColor());
        }
    }
    void OnChessSquareClicked()
    {
        GameManager.instance.boardViewController.TryMove(mSquare);
    }
    private void OnDisable()
    {
        SquareButton.onClick.RemoveListener(OnChessSquareClicked);
    }
    void OnSquareStateChanged()
    {
        if (mSquare.GetSquareState() == SQUARE_STATE.NONE)
        {
            ColorBlock colorBlock = SquareButton.colors;
            colorBlock.normalColor = new Color(1f, 1f, 1f, 5/255f);
            colorBlock.highlightedColor = new Color(1f, 1f, 1f, 5 / 255f);
            colorBlock.selectedColor = new Color(1f, 1f, 1f, 5 / 255f); ;
            SquareButton.colors = colorBlock;
        }
        if (mSquare.GetSquareState() == SQUARE_STATE.HIGHLIGHT)
        {
            ColorBlock colorBlock = SquareButton.colors;
            colorBlock.normalColor = new Color(1f, 1f, 0.2f, 0.5f);
            colorBlock.highlightedColor = new Color(1f, 1f, 0.2f, 0.5f);
            colorBlock.selectedColor = new Color(1f, 1f, 0.2f, 0.5f);
            SquareButton.colors = colorBlock;
        }
        if (mSquare.GetSquareState() == SQUARE_STATE.ATTACK)
        {
            ColorBlock colorBlock = SquareButton.colors;
            colorBlock.normalColor = new Color(1f, 0.1f, 0.1f, 0.5f);
            colorBlock.highlightedColor = new Color(1f, 0.1f, 0.1f, 0.5f);
            colorBlock.pressedColor = new Color(1f, 0.1f, 0.1f, 0.5f);
            SquareButton.colors = colorBlock;
        }
        if (mSquare.GetSquareState() == SQUARE_STATE.SELECTED)
        {
            ColorBlock colorBlock = SquareButton.colors;
            colorBlock.normalColor = new Color(1f, 0f, 0f, 0.5f);
            colorBlock.highlightedColor = new Color(1f, 0f, 0f, 0.5f);
            colorBlock.selectedColor = new Color(1f, 0f, 0f, 0.5f);
            SquareButton.colors = colorBlock;
        }
    }
    void OnTroopChanged()
    {
        if (mSquare.Troop.GetPiece() == PIECE.NONE)
        {
            TroopImage.gameObject.SetActive(false);
        }
        else
        {
            TroopImage.sprite = GameManager.instance.assetManager.
                GetTroopImage(mSquare.Troop.GetPiece(), mSquare.Troop.GetPieceColor());
            TroopImage.gameObject.SetActive(true);

        }
    }
}
