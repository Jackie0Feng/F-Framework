using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class VisualEffectMgr : SingletonMono<VisualEffectMgr>
    {


        public ParticleSystem PlayImmediatelyEffect(ParticleSystem particle, Vector3 pos, Transform parent = null, float time = 1, bool imStop = true)
        {
            ParticleSystem effect = PlayEffect(particle, pos, parent);
            StopEffect(effect, 1);
            return effect;
        }

        public ParticleSystem PlayEffect(ParticleSystem particle, Vector3 positon, Transform parent = null)
        {
            //ParticleSystem effect = Instantiate<ParticleSystem>(particle, positon, Quaternion.identity, parent);
            ParticleSystem particleInstance = PoolMgr.Instance.GetGameObject(particle.gameObject).GetComponent<ParticleSystem>();
            if (parent != null)
                particleInstance.transform.parent = parent;
            particleInstance.transform.position = positon;
            particleInstance.Clear();
            particleInstance.Play();
            return particleInstance;
        }

        public void StopEffect(ParticleSystem particle, float time = 0, bool imStop = false)
        {
            StartCoroutine(StopEffectCoroutine(particle, time, imStop));
        }

        private IEnumerator StopEffectCoroutine(ParticleSystem particle, float time, bool imStop)
        {
            yield return new WaitForSeconds(time);
            particle.Stop();
            if (!imStop)
                yield return new WaitForSeconds(1f);
            PoolMgr.Instance.ReleaseToPool(particle.gameObject);
        }
    }
}
