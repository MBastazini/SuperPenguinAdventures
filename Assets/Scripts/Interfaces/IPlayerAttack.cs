using System;
using UnityEngine;

public interface IPlayerAttack
{
    float getLoadedPercentage();

    float getAttackTime();

    int getAttackDamage();

    int getAttackType(); // 0 = Dash, 1 = Rotating
}
