using UnityEngine;
using System.Collections;
using Battle.Game;

namespace Battle.Weapons
{

    public class MachineGun : BaseWeapon
    {
        protected override void Fire()
        {
            if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out var hit, _objectDistance))
            {
                _settings.weaponMuzzle.LookAt(hit.point);
            }
            else
            {
                _settings.weaponMuzzle.LookAt(_cam.transform.position + (_cam.transform.forward * 50f));
            }

            // ’e‚ð”­ŽË‚·‚é
            var shotDir = GetShotDirectionWithinSpread(_settings.weaponMuzzle);
            var bullet = Instantiate(_settings.bulletPrefab, _settings.weaponMuzzle.position, Quaternion.LookRotation(shotDir));
            bullet.Shoot(new Damage(_parameter.damage, this.gameObject, false));

        }
    }

}