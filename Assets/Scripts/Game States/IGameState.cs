using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState {

    void Start();

    void Update();

    void ResetKart();
}
