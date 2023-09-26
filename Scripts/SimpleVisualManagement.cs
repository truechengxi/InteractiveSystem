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
        [Header("Editor Property")] public float screenDebugDis = 2;
        public bool showDebugGUI;

        private void OnDrawGizmos()
        {
            if (!viewCamera) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(viewCamera.transform.position, boundBoxSize);
            // 将激活范围绘制到屏幕上 
            var offset = (Vector2.one - activeAreaPercentage) / 2;
            var p5 = viewCamera.ViewportToWorldPoint(new Vector3(offset.x, offset.y, screenDebugDis));
            var p6 = viewCamera.ViewportToWorldPoint(new Vector3(1 - offset.x, offset.y, screenDebugDis));
            var p7 = viewCamera.ViewportToWorldPoint(new Vector3(1 - offset.x, 1 - offset.y, screenDebugDis));
            var p8 = viewCamera.ViewportToWorldPoint(new Vector3(offset.x, 1 - offset.y, screenDebugDis));
            Gizmos.DrawLine(p5, p6);
            Gizmos.DrawLine(p6, p7);
            Gizmos.DrawLine(p7, p8);
            Gizmos.DrawLine(p8, p5);
        }

        private void OnDrawGizmosSelected()
        {
        }

        private void OnGUI()
        {
            if (!showDebugGUI) return;
            GUILayout.Label("Select Object:" + Selected);
            // 打印所有可见对象的状态
            foreach (var (visible, state) in VisualStates)
            {
                GUILayout.Label(visible.Name + ":" + state);
            }
        }
#endif
    }
}