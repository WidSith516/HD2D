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
        // ��ȡ��ǰ����״̬����Ϣ
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // ��ȡ��ǰ��������ʱ������һ��ʱ��Ϊ1��
        float animationLength = currentState.length;

        // ��ȡ��ǰ�������Ѳ���ʱ��
        float elapsedTime = currentState.normalizedTime * animationLength;

        // ��ȡ��ǰ������δ����ʱ��
        float remainingTime = animationLength - elapsedTime;

        Debug.Log("�Ѳ���ʱ��: " + elapsedTime + "��");
        Debug.Log("δ����ʱ��: " + remainingTime + "��");
    }
}
