using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombo : MonoBehaviour
{
    public bool isAttacking;
    public int attackCombo;

    private void Start_Combo()
    {
        isAttacking = false;

        if (attackCombo <= 3)
        {
            attackCombo++;
        }
    }

    private void Finish_Combo()
    {
        isAttacking = false;
        attackCombo = 0;
    }

    public bool Attacking()
    {
        return isAttacking;
    }

    public int ComboNumber()
    {
        return attackCombo;
    }
}
