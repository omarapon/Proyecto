using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeting : MonoBehaviour
{
    private void Awake(){

    }
    private void RepositionBackgroung(){
        transform.Translate(53.4f,0,0);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -26){
            RepositionBackgroung();
        }
    }
}
