using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneStopAndSkip : MonoBehaviour
{
    public PlayableDirector cutScene;

    public void StopCutScene(){     //글은 재생됌 어쩔 수 없음. DOTween Start문에서 실행하는거라서 글을 멈추는 방법은 없음. 컷씬은 멈춤.
        cutScene.Pause();
    }

    public void StartCutScene(){
        cutScene.Resume();
    }
}
