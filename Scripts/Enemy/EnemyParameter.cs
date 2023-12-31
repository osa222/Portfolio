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
        public string NAME; //キャラ・敵名
        public EnemyType Type;
        public int MAXHP; //最大HP
        public int ATK; //攻撃力
        [Header("アジリティ、移動速度")]
        public float AGI; //移動速度
        public int LV; //レベル
        public int GETEXP; //取得経験値
        public float AttackRange;
    }


}