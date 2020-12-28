using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace CefSharpDemo
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisibleAttribute(true)]
    public class JSObject
    {
        /// <summary>
        /// Map控件接口，用于JS调用C#
        /// </summary>
        public IMapCtrl MapCtrl { get; set; }

        /// <summary>
        /// 点双击
        /// </summary>
        public void MarkerDoubleClick(string dataIds)
        {
            MapCtrl.MarkerDoubleClick(dataIds);
        }

        /// <summary>
        /// 地图加载完毕
        /// </summary>
        public void MapLoaded()
        {
            MapCtrl.MapLoaded();
        }

        /// <summary>
        /// 图层加载完毕
        /// </summary>
        public void LayerLoaded()
        {
            MapCtrl.LayerLoaded();
        }

        /// <summary>
        /// 绘图结束
        /// </summary>
        /// <param name="drawType">1：多边形选择 2：线选 3：矩形选择(框选) 4：圈选 5：选择位置</param>
        public void DrawEnd(int drawType)
        {
            MapCtrl.DrawEnd(drawType);
        }

        /// <summary>
        /// 通过画图形向WPF控件中添加选择的点
        /// </summary>
        public void AddSelectedByDraw(string dataIds)
        {
            MapCtrl.AddSelectedByDraw(dataIds);
        }

        /// <summary>
        /// 通过点选向WPF控件中添加选择的点
        /// </summary>
        public void AddSelectedByPoint(string dataIds)
        {
            MapCtrl.AddSelectedByPoint(dataIds);
        }

        /// <summary>
        /// 点的点击事件(案件、追踪任务、警情等)
        /// </summary>
        public void PointClick(string dataId, int layerIndex)
        {
            MapCtrl.PointClick(dataId, layerIndex);
        }

        /// <summary>
        /// 选择位置
        /// </summary>
        public void LocationSelected(double lng, double lat)
        {
            MapCtrl.LocationSelected(lng, lat);
        }

        /// <summary>
        /// Map的MoveEnd事件
        /// </summary>
        public void MapMoveEnd(int mapZoom)
        {
            MapCtrl.MapMoveEnd(mapZoom);
        }

        /// <summary>
        /// 最短路径规划
        /// </summary>
        public void ShortestPath(string data)
        {
            MapCtrl.ShortestPath(data);
        }

        /// <summary>
        /// 视频追踪(画箭头线完成)
        /// </summary>
        public void VideoTrackDrawEnd(double lng1, double lat1, double lng2, double lat2)
        {
            MapCtrl.VideoTrackDrawEnd(lng1, lat1, lng2, lat2);
        }

    }
}
