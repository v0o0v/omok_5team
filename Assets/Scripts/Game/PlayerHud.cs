using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Omok
{
    public class PlayerHud : MonoBehaviour
    {
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
