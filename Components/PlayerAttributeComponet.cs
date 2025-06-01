using OpenGLStudy;
using OpenGLStudy.Components;

namespace OpenGLStudy.Components
{
    /// <summary>
    /// 属性组件：用于记录生命值、攻击力等
    /// </summary>
    internal class PlayerAttributeComponent : Component
    {
        public int Health { get; set; }
        public int Attack { get; set; }
        // 可扩展更多属性

        public PlayerAttributeComponent(int health = 100, int attack = 10)
        {
            Health = health;
            Attack = attack;
        }
    }
}
