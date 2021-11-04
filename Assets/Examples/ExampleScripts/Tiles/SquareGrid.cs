﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Cosmos.Test
{
    public struct SquareGrid
    {
        public struct Square
        {
            public float CenterX { get; private set; }
            public float CenterY { get; private set; }
            public float SideLength { get; private set; }
            public float Top { get; private set; }
            public float Bottom { get; private set; }
            public float Left { get; private set; }
            public float Right { get; private set; }
            public float HalfSideLength { get; private set; }
            public Square(float centerX, float centerY, float sideLength)
            {
                CenterX = centerX;
                CenterY = centerY;
                SideLength = sideLength;
                HalfSideLength = sideLength * 0.5f;

                Top = centerY + HalfSideLength;
                Bottom = centerY - HalfSideLength;
                Left = centerX - HalfSideLength;
                Right = CenterX + HalfSideLength;
            }
            public bool Contains(float x, float y)
            {
                if (x < Left || x > Right) return false;
                if (y > Top || y < Bottom) return false;
                return true;
            }
            public static readonly Square Zero = new Square(0, 0, 0);
            public static readonly Square One = new Square(1, 1, 1);
        }
        Square[,] squareRects;
        public float CenterX { get; private set; }
        public float CenterY { get; private set; }
        public uint CellSection { get; private set; }
        public float SquareSideLength { get; private set; }
        public float SquareTop { get; private set; }
        public float SquareBottom { get; private set; }
        public float SquareLeft { get; private set; }
        public float SquareRight { get; private set; }
        public float HalfSideLength { get; private set; }
        public float CellSideLength { get; private set; }
        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }
        /// <summary>
        /// 缓冲去范围；
        /// 缓冲区=bufferRange+border;
        /// </summary>
        public float BufferZoneRange { get; private set; }
        public SquareGrid(float cellSideLength, uint cellSection, float offsetX, float offsetY) : this(cellSideLength, cellSection, offsetX, offsetY, 0) { }
        public SquareGrid(float cellSideLength, uint cellSection, float offsetX, float offsetY, float bufferRange)
        {
            if (cellSideLength < 0)
                throw new OverflowException("cellSideLength can not less than zero !");
            CellSection = cellSection;

            this.OffsetX = offsetX;
            this.OffsetY = offsetY;

            SquareSideLength = cellSideLength * cellSection;
            CellSideLength = cellSideLength;
            squareRects = new Square[cellSection, cellSection];
            HalfSideLength = SquareSideLength / 2;

            CenterX = HalfSideLength + offsetX;
            CenterY = HalfSideLength + offsetY;

            SquareLeft = CenterX - HalfSideLength;
            SquareRight = CenterX + HalfSideLength;

            SquareTop = CenterY + HalfSideLength;
            SquareBottom = CenterY - HalfSideLength;

            BufferZoneRange = bufferRange;
            CreateRectangles(offsetX, offsetY);
        }
        public Square GetRectangle(float posX, float posY)
        {
            if (!IsOverlapping(posX, posY))
                return Square.Zero;
            var col = (posX - OffsetX) / CellSideLength;
            var row = (posY - OffsetY) / CellSideLength;
            return squareRects[(int)col, (int)row];
        }
        public Square[] GetRectanglesByBufferZone(float posX, float posY)
        {
            if (!IsOverlappingBufferZone(posX, posY))
                return new Square[0];
            var col = (int)((posX - OffsetX) / CellSideLength);
            var row = (int)((posY - OffsetY) / CellSideLength);
            if (!IsOverlapping(posX, posY))
                return new Square[0];
            Square[] bufferZoneSquares = new Square[5];
            bufferZoneSquares[0] = squareRects[col, row];
            int idx = 1;
            var leftCol = col - 1;
            var rightCol = col + 1;
            var upRow = row + 1;
            var downRow = row - 1;

            if (leftCol >= 0)
                bufferZoneSquares[idx++] = squareRects[leftCol, row];
            if (rightCol < CellSection)
                bufferZoneSquares[idx++] = squareRects[rightCol, row];
            if (upRow < CellSection)
                bufferZoneSquares[idx++] = squareRects[col, upRow];
            if (downRow >= 0)
                bufferZoneSquares[idx++] = squareRects[col, downRow];
            var srcSquares = new Square[idx];
            int dstIdx = 0;
            for (int i = 0; i < idx; i++)
            {
                if (IsOverlappingBufferZoneRange(bufferZoneSquares[i], posX, posY))
                {
                    srcSquares[dstIdx] = bufferZoneSquares[i];
                    dstIdx++;
                }
            }
            var dstSquares = new Square[dstIdx];
            Array.Copy(srcSquares, 0, dstSquares, 0, dstIdx);
            return dstSquares;
        }
        public Square[,] GetAllRectangle()
        {
            return squareRects;
        }
        public bool IsOverlapping(float posX, float posY)
        {
            if (posX < SquareLeft || posX > SquareRight) return false;
            if (posY < SquareBottom || posY > SquareTop) return false;
            return true;
        }
        bool IsOverlappingBufferZone(float posX, float posY)
        {
            if (posX < SquareLeft - BufferZoneRange || posX > SquareRight + BufferZoneRange) return false;
            if (posY < SquareBottom - BufferZoneRange || posY > SquareTop + BufferZoneRange) return false;
            return true;
        }
        bool IsOverlappingBufferZoneRange(Square square, float posX, float posY)
        {
            if (posX < square.Left - BufferZoneRange || posX > square.Right + BufferZoneRange) return false;
            if (posY < square.Bottom - BufferZoneRange || posY > square.Top + BufferZoneRange) return false;
            return true;
        }
        void CreateRectangles(float offsetX, float offsetY)
        {
            var centerOffsetX = CellSideLength / 2 + offsetX;
            var centerOffsetY = CellSideLength / 2 + offsetY;
            for (int i = 0; i < CellSection; i++)
            {
                for (int j = 0; j < CellSection; j++)
                {
                    squareRects[i, j] = new Square(i * CellSideLength + centerOffsetX, j * CellSideLength + centerOffsetY, CellSideLength);
                }
            }
        }
    }
}