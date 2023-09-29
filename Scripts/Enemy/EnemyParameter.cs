using UnityEngine;

namespace Battle
{

    public enum EnemyType
    {
        Fly, Walk
    }

    [CreateAssetMenu]
    public class EnemyParameter : ScriptableObject
    {
        public string NAME; //�L�����E�G��
        public EnemyType Type;
        public int MAXHP; //�ő�HP
        public int ATK; //�U����
        public int AGI; //�ړ����x
        public int LV; //���x��
        public int GETEXP; //�擾�o���l
        public float AttackRange;
    }


}