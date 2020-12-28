using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharpDemo
{
    /// <summary>
    /// Map控件接口，用于JS调用C#
    /// </summary>
    public interface IMapCtrl
    {
        /// <summary>
        /// 点双击
        /// </summary>
        void MarkerDoubleClick(string dataIds);
        /// <summary>
        /// 地图加载完毕
        /// </summary>
        void MapLoaded();
        /// <summary>
        /// 图层加载完毕
        /// </summary>
        void LayerLoaded();
        /// <summary>
        /// 绘图结束
        /// </summary>
        /// <param name="drawType">1：多边形选择 2：线选 3：矩形选择(框选) 4：圈选 5：选择位置</param>
        void DrawEnd(int drawType);
        /// <summary>
        /// 通过画图形向WPF控件中添加选择的点
        /// </summary>
        void AddSelectedByDraw(string dataIds);
        /// <summary>
        /// 通过点选向WPF控件中添加选择的点
        /// </summary>
        void AddSelectedByPoint(string dataIds);
        /// <summary>
        /// 点的点击事件
        /// </summary>
        void PointClick(string dataId, int layerIndex);
        /// <summary>
        /// 选择位置
        /// </summary>
        void LocationSelected(double lng, double lat);
        /// <summary>
        /// Map的MoveEnd事件
        /// </summary>
        void MapMoveEnd(int mapZoom);
        /// <summary>
        /// 最短路径规划
        /// </summary>
        void ShortestPath(string data);
        /// <summary>
        /// 视频追踪(画箭头线完成)
        /// </summary>
        void VideoTrackDrawEnd(double lng1, double lat1, double lng2, double lat2);
    }
}
