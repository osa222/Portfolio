using UnityEngine;

namespace Battle
{

    public enum EnemyType
    {
        Fly, Ground
    }

    [CreateAssetMenu]
    public class EnemyParameter : ScriptableObject
    {
        public string NAME; //�L�����E�G��
        public EnemyType Type;
        public int MAXHP; //�ő�HP
        public int ATK; //�U����
        [Header("�A�W���e�B�A�ړ����x")]
        public float AGI; //�ړ����x
        public int LV; //���x��
        public int GETEXP; //�擾�o���l
        public float AttackRange;
    }


}