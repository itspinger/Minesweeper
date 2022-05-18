using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
internal class Debuger : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        button.onClick.AddListener(() => Debug.Log("wtf"));
    }

}
