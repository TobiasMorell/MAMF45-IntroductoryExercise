using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Instrument : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _description;
        private AudioSource _audioSource;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public string Description
        {
            get { return _description; }
        }

        public void Interact()
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();
            _audioSource.Play();
        }
    }
}
