using UnityEngine;

namespace InteractiveSystem
{
    /// <summary>
    /// 一个简单的可视化管理器的实现
    /// </summary>
    public class SimpleVisualManagement : VisualManagementSingleSelection<ISimpleVisible>
    {
        public Camera viewCamera;

        public RectTransform canvasTransform;

        public LayerMask occlusionMask;

        [Range(0, 1)] public float rayCastDisOffset;

        public Vector3 boundBoxSize;

        // TODO:等待优化
        public Rect screenActiveArea;


        private Bounds _bounds;

        private Vector2 _screenCenter;

        private float _selectedSqrDisBuf;

        protected override void Update()
        {
            // TODO: 等待优化
            // 刷新绑定盒的参数
            _bounds.size = boundBoxSize;
            _bounds.center = transform.position;
            // 计算屏幕中心坐标
            _screenCenter = canvasTransform.rect.size / 2;

            base.Update();
        }

        protected override VisualState UpdateVisualState(ISimpleVisible visible)
        {
            var pos = visible.Position;
            if (!_bounds.Contains(pos))
                return VisualState.Invisible;

            // 检测是否在屏幕内
            var screenPos = viewCamera.WorldToViewportPoint(visible.Position);
            if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1 || screenPos.z < 0)
                return VisualState.Invisible;

            // 检测是否有遮挡物
            var cameraPos = viewCamera.transform.position;
            var dir = pos - cameraPos;
            var dis = Mathf.Max(0, pos.magnitude - rayCastDisOffset);
            var res = Physics.Raycast(cameraPos, dir, dis, occlusionMask);
#if UNITY_EDITOR
            Debug.DrawRay(cameraPos, dir, res ? Color.red : Color.green);
#endif
            if (res)
                return VisualState.Invisible;

            // TODO: 改称相对形式
            // 检测是否在可选择区域内
            // if (!screenActiveArea.Contains(screenPos))
            //     return VisualState.Selectable;

            // 选择出最近的对象
            var sqrDis = ((Vector2)screenPos - _screenCenter).sqrMagnitude;
            if (Selected != null && sqrDis >= _selectedSqrDisBuf) return VisualState.Selectable;
            _selectedSqrDisBuf = sqrDis;
            return VisualState.Selected;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!viewCamera) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(viewCamera.transform.position, boundBoxSize);
        }

        private void OnDrawGizmosSelected()
        {
        }
#endif
    }
}