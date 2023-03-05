using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DduduTest))]
public class DduduTestEditor : Editor
{
    List<string> _dduduTypes = new List<string>();
    List<string> _animClips = new List<string>();
    int _dduduSelectIndex = 0;
    int _animSelectIndex = 0;
    
	public override void OnInspectorGUI()
	{

        base.OnInspectorGUI();
        var tester = target as DduduTest;

        if (Application.isPlaying)
        {
            foreach (var item in tester.infos)
            {
                if (GUILayout.Button($"{item.name} 뚜두 부르기"))
                {
                    tester.SpawnDdudu(item);
                }
            }

            for (int typeNum = 101; typeNum <= 110; typeNum++)
            {
                _dduduTypes.Add(((DduduTest.DduduType)typeNum).ToString());
            }
            _dduduSelectIndex = EditorGUILayout.Popup(_dduduSelectIndex, _dduduTypes.ToArray());
            
            var testerObj = tester._ddudus[_dduduSelectIndex];
            if (testerObj != null)
            {
                testerObj.TryGetComponent<Animator>(out Animator _animator);
                var clips = _animator.runtimeAnimatorController.animationClips;
                if (_animClips.Count > 0)
                {
                    _animClips.Clear();
                }
                foreach (var clip in clips)
                {
                    _animClips.Add(clip.ToString());
                }
                _animSelectIndex = EditorGUILayout.Popup(_animSelectIndex, _animClips.ToArray());

                if (GUILayout.Button("Play Animation"))
                {
                    tester.StartAnimation(_dduduSelectIndex, clips[_animSelectIndex].name);
                } 
            }

            if (GUILayout.Button("Stop Animation"))
            {
                tester.StopAnimation(_dduduSelectIndex);
            }
        }  
	}
}

#endif



public class DduduTest : MonoBehaviour
{
    public enum DduduType
    {
        감자 = 101,
        포도,
        망고,
        방울,
        분홍,
        토마토,
        스타,
        호박,
        용과,
        납복
    }
    
    [HideInInspector] public IEnumerable<DduduInfo> infos;
    [HideInInspector] public Ddudu[] _ddudus = new Ddudu[10];
    private GameObject curDdudu = null;

    private void Start() 
    {
        XMLParser.LoadParseXML<DduduManager, DduduInfo, DduduData>("Data/DduduFile");
        infos = DduduManager.Instance.GetInfoEnumerable();
    }

    public void SpawnDdudu(DduduInfo info)
    {
        if (_ddudus[info.code - 101] != null)
        {
            DduduManager.Instance.RemoveData(_ddudus[info.code - 101].data.id);
            var obj = _ddudus[info.code - 101];
            _ddudus[info.code - 101] = null;
            Destroy(obj.gameObject);
        }
        if (curDdudu != null)
        {
            curDdudu.SetActive(false);
        }
        
        _ddudus[info.code - 101] = DduduSpawner.SpawnDdudu(info.code, Vector3.zero, true);
        _ddudus[info.code - 101].StopMove();
        curDdudu = _ddudus[info.code - 101].gameObject;
    }

    public void StopAnimation(int dduduIndex)
    {
        _ddudus[dduduIndex].StopMove();
    }

    public void StartAnimation(int dduduIndex, string animName)
    {
        _ddudus[dduduIndex].TryGetComponent<Animator>(out var animator);
        if (animName.Substring(0, 4).Equals("Idle"))
        {
            animator.SetLayerWeight(1, 0);
        }
        else 
        {
            animator.SetLayerWeight(1, 1);
        }
        animator.Play(animName.Substring(0, animName.Length-4));
    }

    private void OnDisable() 
    {
        for (int index = 0; index < 10; index++)
        {
            if (_ddudus[index] != null)
            {
                DduduManager.Instance.RemoveData(_ddudus[index].data.id);
            }
        }
    }
}
