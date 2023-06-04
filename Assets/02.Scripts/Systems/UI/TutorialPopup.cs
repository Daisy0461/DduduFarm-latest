using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : DefaultPanel
{
    [SerializeField] private Sprite[] _tutorialSprites;
    [SerializeField] private GameObject _leftButton;
    [SerializeField] private GameObject _rightButton;
    
    private int _currentSpriteIndex;

	public override void Activate(params object[] objs)
	{
        SetTutorialImage(0);
		base.Activate(objs);
	}

    private void SetTutorialImage(int index)
    {
        _currentSpriteIndex = (index + _tutorialSprites.Length) % _tutorialSprites.Length;  
        _images[0].sprite = _tutorialSprites[_currentSpriteIndex];
        
        _leftButton.SetActive(0 < _currentSpriteIndex);
        _rightButton.SetActive(_currentSpriteIndex <_tutorialSprites.Length - 1);
    }

    public void OnLeftButtonClick()
    {
        SetTutorialImage(_currentSpriteIndex - 1);
    }

    public void OnRightButtonClick()
    {
        SetTutorialImage(_currentSpriteIndex + 1);
    }
}
