using UnityEngine;

public class AnimationTimeQuery : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 获取当前动画状态的信息
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // 获取当前动画的总时长（归一化时间为1）
        float animationLength = currentState.length;

        // 获取当前动画的已播放时长
        float elapsedTime = currentState.normalizedTime * animationLength;

        // 获取当前动画的未播放时长
        float remainingTime = animationLength - elapsedTime;

        Debug.Log("已播放时长: " + elapsedTime + "秒");
        Debug.Log("未播放时长: " + remainingTime + "秒");
    }
}
