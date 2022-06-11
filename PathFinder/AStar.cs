using System;
using System.Collections.Generic;

namespace PathFinder {

    public class AStarPoint : IComparable<AStarPoint> {
        public int x;
        public int y;
        public int f;
        public int g;
        public bool open;
        public bool close;
        public AStarPoint parent;

        public AStarPoint (int x = 0, int y = 0) {
            Set (x, y);
            ResetState ();
        }

        public void Set (int x, int y) {
            this.x = x;
            this.y = y;
        }

        public void ResetState () {
            this.f = 0;
            this.g = 0;
            open = false;
            close = false;
            parent = null;
        }

        /**
         *以小表示大  就是最小堆了
         */
        public int CompareTo (AStarPoint other) {
            return other.f - this.f;
        }

        /**
         * 减少gc
         */
        private static Stack<AStarPoint> _pointpool = new Stack<AStarPoint> ();
        public static AStarPoint NewPoint (int x, int y) {
            if (_pointpool.Count > 0) {
                AStarPoint point = _pointpool.Pop ();
                point.Set (x, y);
                point.ResetState ();
                return point;
            }
            return new AStarPoint (x, y);
        }

        public static void DelPoint (AStarPoint point) {
            _pointpool.Push (point);
        }

    }

    public class AStar : IPathFinder {
        private int _helperwidth = 100;
        private int _helperHeight = 100;
        private int _currentwidth = 0;
        private int _currentheight = 0;
        private AStarPoint[, ] _helper = new AStarPoint[100, 100];

        int _usedlength = 0;
        AStarPoint[] _used = new AStarPoint[4096];
        // public List<AStarPoint> _used = new List<AStarPoint>(4096);
        private MaxHeap<AStarPoint> _openlist = new MaxHeap<AStarPoint> (1024);
        private Func<AStarPoint, AStarPoint, int> _g;
        private Func<AStarPoint, AStarPoint, int> _f;

        private int DefaultF (AStarPoint left, AStarPoint right) {
            int delx = Math.Abs (left.x - right.x);
            int dely = Math.Abs (left.y - right.y);
            int delmax = Math.Max (delx, dely);
            int delmin = Math.Min (delx, dely);
            return delmax * 10 + delmin * 4;
            return (int) MathExtra.FastSqrt (delx * delx + dely * dely) * 10;

        }

        private int DefaultG (AStarPoint left, AStarPoint right) {
            if (left.x == right.x) {
                return 10;
            }

            if (left.y == right.y) {
                return 10;
            }

            return 14; // 根号二*10 是一个分水岭  
        }

        public AStar () {
            _f = DefaultF;
            _g = DefaultG;
        }

        public void SetF (Func<AStarPoint, AStarPoint, int> f) {
            _f = f;
        }

        public void SetG (Func<AStarPoint, AStarPoint, int> g) {
            _g = g;
        }

        private void Reset (int width, int height) {
            if (_helper == null || height > _helperHeight || width > _helperwidth) {
                _helperwidth = Math.Max (width, _helperwidth);
                _helperHeight = Math.Max (height, _helperHeight);
                _helper = new AStarPoint[_helperHeight, _helperwidth];
            }

            _currentwidth = width;
            _currentheight = height;

            for (int i = 0; i < _usedlength; ++i) {
                AStarPoint point = _used[i];
                _helper[point.y, point.x] = null;
                AStarPoint.DelPoint (point);
            }
            _usedlength = 0;
            _openlist.Clear ();

            if (_currentwidth * _currentheight > _used.Length) {
                _used = new AStarPoint[_currentwidth * _currentheight];
            }

        }

        private int Direct (AStarPoint start, AStarPoint end) {
            int x = end.x - start.x;
            int y = end.y - start.y;
            for (int dir = 0; dir < (int) PathHelper.MoveDir.COUNT; ++dir) {
                if (x == PathHelper.DIR_VALUE2[dir, 0] && y == PathHelper.DIR_VALUE2[dir, 1]) {
                    return dir;
                }

            }

            throw new Exception ("can not get here");
            return 0;
        }

        private bool BackTraceDirection (AStarPoint start, List<int> steps) {

            AStarPoint first = start;
            while (first.parent != null) {
                AStarPoint parent = first.parent;
                steps.Add (Direct (first, parent));
                first = first.parent;
            }
            return true;
        }

        private bool BackTracePoint (AStarPoint start, List<Point> steps) {

            AStarPoint first = start;
            while (first.parent != null) {
                AStarPoint parent = first.parent;
                steps.Add (new Point (parent.x, parent.y));
                first = first.parent;
            }
            return true;
        }

        AStarPoint GetPoint (int x, int y) {
            if (x >= _currentwidth || y >= _currentheight) {
                return default (AStarPoint);
            }

            try {

                AStarPoint grid = _helper[y, x];
                if (grid == null) {
                    grid = AStarPoint.NewPoint (x, y);
                    _helper[y, x] = grid;
                    _used[_usedlength++] = grid;
                }
                return grid;
            } catch (Exception e) {
                //   #if UNITY_EDITOR
                //Logger.LogError (string.Format ("Exception[{0}]  \n width:{1} height：{2} posx: {3}  posy: {4}",
                //    e.ToString (), _helperwidth, _helperHeight, x, y));
                //    #endif
                return null;
            }
        } 

        private bool IsCloseInObstacle<T> (Point point, T[] obstacle, int width, int height) {

            if(!obstacle[width * point.y + point.x].Equals (default (T))){
//#if UNITY_EDITOR     
//                Logger.LogError (string.Format ("寻路单位在障碍点上"));
//#endif
                return true;
            }

            for (int dir = 0; dir < (int) PathHelper.MoveDir.COUNT; ++dir) {
                int i = point.x + PathHelper.DIR_VALUE2[dir, 0];
                int j = point.y + PathHelper.DIR_VALUE2[dir, 1];
                if (i < 0 || i >= width) {
                    continue;
                }

                if (j < 0 || j >= height) {
                    continue;
                }

                if (obstacle[width * j + i].Equals (default (T))) {
                    return false;
                }

            }

            return true;
        }

        private bool SearchPath<T> (T[] obstacle, int width, int height, Point pstart, Point pend) {

            if (pstart.x < 0 || pstart.y < 0 || pend.x < 0 || pend.y < 0) {
                //#if UNITY_EDITOR   
                //Logger.LogError (string.Format ("寻路坐标 小于0了  {0}-{1}-{2}-{3}-{4}-{5}", pstart.x, pstart.y, pend.x, pend.y,width,height));
                //#endif
                return false;
            }

            if (pstart.x >= width || pstart.y >= height || pend.x>= width || pend.y>= height) {
                //#if UNITY_EDITOR   
                //Logger.LogError (string.Format ("寻路坐标 越界  {0}-{1}-{2}-{3}-{4}-{5}", pstart.x, pstart.y, pend.x, pend.y,width,height));
                //#endif
                return false;
            }

            // 先处理出发点和目标点四周都是障碍的情况
            if (IsCloseInObstacle<T> (pstart, obstacle, width, height)) {
                return false;
            }
            if (IsCloseInObstacle<T> (pend, obstacle, width, height)) {
                return false;
            }

            Reset (width, height);

            AStarPoint fstart = GetPoint (pend.x, pend.y);
            AStarPoint fend = GetPoint (pstart.x, pstart.y);

            if (fstart == null || fend == null) {
                return false;
            }

            _openlist.Add (fstart);

            while (_openlist.Count > 0) {
                AStarPoint open = _openlist.Pop ();
                open.close = true;

                for (int dir = 0; dir < (int) PathHelper.MoveDir.COUNT; ++dir) {
                    int i = open.x + PathHelper.DIR_VALUE2[dir, 0];
                    int j = open.y + PathHelper.DIR_VALUE2[dir, 1];

                    if (i < 0 || i >= width) {
                        continue;
                    }

                    if (j < 0 || j >= height) {
                        continue;
                    }
                    // 如果这个位置是目标点   就不管障碍了 
                    if (!obstacle[width * j + i].Equals (default (T)) && (i != fend.x || j != fend.y)) {
                        continue;
                    }

                    AStarPoint grid = GetPoint (i, j);

                    if (grid == null || grid.close) {
                        continue;
                    }

                    int g = open.g + _g (open, grid);
                    if (!grid.open || grid.g > g) {

                        grid.g = g;
                        grid.f = grid.g + _f (fend, grid);
                        grid.parent = open;
                        if (!grid.open) {
                            grid.open = true;
                            _openlist.Add (grid);

                            // 找到了
                            if (i == fend.x && j == fend.y) {
                                return true;
                            }
                        } else {
                            _openlist.RebuildElement (grid);
                        }
                    }
                }

            }

            return false;
        }

        public bool Do<T> (T[] obstacle, int width, int height, Point pstart, Point pend, List<int> steps) {

            steps.Clear ();
            if (pstart.x == pend.x && pstart.y == pend.y) {
                return true;
            }

            if (SearchPath (obstacle, width, height, pstart, pend)) {
                AStarPoint grid = GetPoint (pstart.x, pstart.y);
                return BackTraceDirection (grid, steps);
            }

            return false;
        }

        public bool Do<T> (T[] obstacle, int width, int height, Point pstart, Point pend, List<Point> steps) {
            steps.Clear ();
            if (pstart.x == pend.x && pstart.y == pend.y) {
                return true;
            }

            if (SearchPath (obstacle, width, height, pstart, pend)) {
                AStarPoint grid = GetPoint (pstart.x, pstart.y);
                return BackTracePoint (grid, steps);
            }

            return false;
        }

    }
}