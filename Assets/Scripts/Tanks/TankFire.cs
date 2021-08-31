using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Projectiles;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Tanks {
    public class TankFire : MonoBehaviour {
        [Header("Necessary Bullet Details")]
        [SerializeField] private Transform _turretFirePos = null;
        [SerializeField] private Bullet _bulletToFire = null;
        [Header("Feedback")]
        [SerializeField] private AudioClip _fireSound = null;
        [SerializeField] private ParticleSystem _fireParticles = null;
        [SerializeField] private int _fireParticleCount = 50;
        [Header("Ignore Collision When Firing")]
        [SerializeField] private Collider _ignoreCollider = null;
        [SerializeField] private float _ignoreDuration = 0.2f;
        [Header("Limits")]
        [SerializeField] private bool _hasMaximumBullets = false;
        [SerializeField] private float _maximumBullets = 5;

        private List<Bullet> _bullets = new List<Bullet>();

        private void Awake()
        {
            if (_bulletToFire == null || _turretFirePos == null) {
                DebugHelper.Error(gameObject, "Missing Bullet Information To Fire");
            }
            // Ensure collect particles don't play on awake or self destruct
            if (_fireParticles != null && _fireParticles.gameObject.activeInHierarchy) {
                _fireParticles.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _bullets.Clear();
        }

        public void Fire()
        {
            if (_bulletToFire == null || _turretFirePos == null) return;

            _bullets = _bullets.Where(item => item != null).ToList();
            if (_hasMaximumBullets && _bullets.Count >= _maximumBullets) return;

            Bullet bullet = Instantiate(_bulletToFire.gameObject, _turretFirePos.position, _turretFirePos.rotation).GetComponent<Bullet>();
            if (_ignoreCollider != null) {
                bullet.TemporaryIgnore(_ignoreCollider, _ignoreDuration);
            }
            _bullets.Add(bullet);
            FireFeedback();
        }

        private void FireFeedback()
        {
            if (_fireParticles != null) {
                Instantiate(_fireParticles, _turretFirePos.position, _turretFirePos.rotation).gameObject.SetActive(true);
            }
            // Audio (TODO: Consider Object Pooling for performance)
            if (_fireSound != null) {
                AudioHelper.PlayClip2D(_fireSound);
            }
        }
    }
}