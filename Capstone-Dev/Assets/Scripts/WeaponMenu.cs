
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine.UI;

namespace AssemblyCSharp
{
    [System.Serializable]
    public class WeaponMenu
    {
        public Weapon Weapons = null;

        public Image WepPanel = null;
        public Image WeaponSprite = null;
        public Image AmmoBar = null;
        public Text AmmoValue = null;
        public Image ReloadImg = null;

        //update current weapon menu
        public void UpdateWeaponMenu(Weapon CurrentWeapon)
        {
            if (CurrentWeapon != null)
            {
                //enable current menu
                WepPanel.gameObject.SetActive(true);
                WepPanel.color = Color.yellow;

                //weapon sprite
                WeaponSprite.sprite = CurrentWeapon.WeaponIcon;

                //ammo text
                AmmoValue.text = CurrentWeapon.CurrentAmmos.ToString() + " / " + CurrentWeapon.AmmoSize;

                //ammo bar % shown
                AmmoBar.fillAmount = (float)CurrentWeapon.CurrentAmmos / (float)CurrentWeapon.AmmoSize;

                
                ReloadImg.fillAmount = 0f;
                /*
                //check if it's reloading current weapon
                if (IsReloading && CurrentWeapon == ListWeapons[vCurWeapIndex])
                    CurrentMenu.ReloadImg.fillAmount = ((float)TimeToReload / 1f);
                else if (!IsReloading && CurrentWeapon.vAmmoCur == 0)
                    CurrentMenu.ReloadImg.fillAmount = 1f;
                else
                    CurrentMenu.ReloadImg.fillAmount = 0f;
                 */

            }
            else
            {
                //disable the whole menu if there is no weapon there
                WepPanel.gameObject.SetActive(false);
            }
        }
    }
}
