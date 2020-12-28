using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CefSharpDemo
{
    /// <summary>
    /// Map控件接口实现，用于JS调用C#
    /// </summary>
    public class MapCtrl : UserControl, IMapCtrl
    {
        /// <summary>
        /// 可以选择像机的最小mapZoom
        /// </summary>
        protected int _selectionMapZoom = 15;

        /// <summary>
        /// 点双击
        /// </summary>
        public virtual void MarkerDoubleClick(string dataIds) { }
        /// <summary>
        /// 地图加载完毕
        /// </summary>
        public virtual void MapLoaded() { }
        /// <summary>
        /// 图层加载完毕
        /// </summary>
        public virtual void LayerLoaded() { }
        /// <summary>
        /// 绘图结束
        /// </summary>
        /// <param name="drawType">1：多边形选择 2：线选 3：矩形选择(框选) 4：圈选</param>
        public virtual void DrawEnd(int drawType) { }
        /// <summary>
        /// 通过画图形向WPF控件中添加选择的点
        /// </summary>
        public virtual void AddSelectedByDraw(string dataIds) { }
        /// <summary>
        /// 通过点选向WPF控件中添加选择的点
        /// </summary>
        public virtual void AddSelectedByPoint(string dataIds) { }
        /// <summary>
        /// 点的点击事件
        /// </summary>
        public virtual void PointClick(string dataId, int layerIndex) { }
        /// <summary>
        /// 选择位置
        /// </summary>
        public virtual void LocationSelected(double lng, double lat) { }
        /// <summary>
        /// Map的MoveEnd事件
        /// </summary>
        public virtual void MapMoveEnd(int mapZoom) { }
        /// <summary>
        /// 最短路径规划
        /// </summary>
        public virtual void ShortestPath(string data) { }
        /// <summary>
        /// 视频追踪(画箭头线完成)
        /// </summary>
        public virtual void VideoTrackDrawEnd(double lng1, double lat1, double lng2, double lat2) { }

    }
}
