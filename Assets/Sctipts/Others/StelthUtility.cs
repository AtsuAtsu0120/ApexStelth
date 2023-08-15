using UnityEngine;

public static class StelthUtility
{
    /// <summary>
    /// ���C�L���X�g�����������I�u�W�F�N�g�̌����Ă���������擾
    /// </summary>
    /// <param name="hit">���C�L���X�g</param>
    /// <returns></returns>
    public static Side GetFacingSide(RaycastHit hit)
    {
        return Vector3.SignedAngle(hit.transform.forward, hit.normal, Vector3.up) switch
        {
            < -135 => Side.Backward,
            < -45 => Side.Left,
            < 45 => Side.Forward,
            < 135 => Side.Right,
            <= 180 => Side.Backward,
            _ => Side.Error
        };
    }
    /// <summary>
    /// �����������Ă���ʂ̒��S����[�܂ł̋������擾����B
    /// </summary>
    /// <param name="facingSide">�����Ă���ʂ̏��</param>
    /// <param name="transform">���������߂�I�u�W�F�N�g��Transform</param>
    /// <returns></returns>
    public static float GetObjectEdgeDirection(Side facingSide, Transform transform)
    {
        var coverOffset = facingSide switch
        {
            Side.Forward => transform.right * (transform.localScale.x / 2.0f) * -1.0f,
            Side.Left => transform.forward * (transform.localScale.z / 2.0f) * -1.0f,
            Side.Backward => transform.right * (transform.localScale.x / 2.0f) * -1.0f,
            Side.Right => transform.forward * (transform.localScale.z / 2.0f) * -1.0f,
            Side.Error => new Vector3(),
            _ => new Vector3()
        };
        var objectEdgePosition = coverOffset + transform.position;
        return Vector3.Distance(transform.position, objectEdgePosition);
    }
    public static bool IsHitInAngle(Transform hitObject, Transform self, float maxAngle, float minAngle)
    {
        var direction = hitObject.position - self.position;
        var angle = Vector3.Dot(self.forward, direction.normalized);
        return angle > minAngle && angle < maxAngle;
    }
}
