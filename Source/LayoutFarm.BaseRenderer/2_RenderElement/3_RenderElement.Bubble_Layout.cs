﻿// 2015,2014 ,Apache2, WinterDev
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PixelFarm.Drawing;
using LayoutFarm.RenderBoxes;

namespace LayoutFarm
{



    partial class RenderElement
    {

        public virtual void TopDownReCalculateContentSize()
        {
            MarkHasValidCalculateSize();
        }

        internal static void SetCalculatedDesiredSize(RenderBoxBase v, int desiredWidth, int desiredHeight)
        {
            v.b_width = desiredWidth;
            v.b_height = desiredHeight;
            v.MarkHasValidCalculateSize();
        }
        public static bool IsLayoutSuspending(RenderBoxBase re)
        {
            //recursive
            if (re.IsTopWindow)
            {
                return (re.uiLayoutFlags & RenderElementConst.LY_SUSPEND) != 0;
            }
            else
            {

                if ((re.uiLayoutFlags & RenderElementConst.LY_SUSPEND) != 0)
                {

                    return true;
                }
                else
                {

                    var parentElement = re.ParentRenderElement as RenderBoxBase;
                    if (parentElement != null)
                    {
                        return IsLayoutSuspending(parentElement);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        bool IsInLayoutSuspendMode
        {
            get
            {
                return (uiLayoutFlags & RenderElementConst.LY_SUSPEND) != 0;
            }
        }


        internal static int GetLayoutSpecificDimensionType(RenderElement visualElement)
        {
            return visualElement.uiLayoutFlags & 0x3;
        }

        public bool HasCalculatedSize
        {
            get
            {
                return ((uiLayoutFlags & RenderElementConst.LAY_HAS_CALCULATED_SIZE) != 0);
            }
        }
        protected void MarkHasValidCalculateSize()
        {

            uiLayoutFlags |= RenderElementConst.LAY_HAS_CALCULATED_SIZE;
#if DEBUG
            this.dbug_ValidateRecalculateSizeEpisode++;
#endif
        }
        public bool IsInLayoutQueue
        {
            get
            {
                return (uiLayoutFlags & RenderElementConst.LY_IN_LAYOUT_QUEUE) != 0;
            }
            set
            {
                uiLayoutFlags = value ?
                      uiLayoutFlags | RenderElementConst.LY_IN_LAYOUT_QUEUE :
                      uiLayoutFlags & ~RenderElementConst.LY_IN_LAYOUT_QUEUE;
            }
        }
         
        public bool NeedReCalculateContentSize
        {
            get
            {
                return (uiLayoutFlags & RenderElementConst.LAY_HAS_CALCULATED_SIZE) == 0;
            }
        } 

        internal void MarkInvalidContentArrangement()
        {
            uiLayoutFlags &= ~RenderElementConst.LY_HAS_ARRANGED_CONTENT;
#if DEBUG

            this.dbug_InvalidateContentArrEpisode++;
            dbug_totalInvalidateContentArrEpisode++;
#endif
        }
        public void MarkInvalidContentSize()
        {
            uiLayoutFlags &= ~RenderElementConst.LAY_HAS_CALCULATED_SIZE;
#if DEBUG
            this.dbug_InvalidateRecalculateSizeEpisode++;
#endif
        }
        public void MarkValidContentArrangement()
        {

#if DEBUG
            this.dbug_ValidateContentArrEpisode++;
#endif
             
            uiLayoutFlags |= RenderElementConst.LY_HAS_ARRANGED_CONTENT;
        }
        public bool NeedContentArrangement
        {
            get
            {
                return (uiLayoutFlags & RenderElementConst.LY_HAS_ARRANGED_CONTENT) == 0;
            }
        }
        internal bool FirstArrangementPass
        {

            get
            {
                return (propFlags & RenderElementConst.FIRST_ARR_PASS) != 0;
            }
            set
            {
                propFlags = value ?
                   propFlags | RenderElementConst.FIRST_ARR_PASS :
                   propFlags & ~RenderElementConst.FIRST_ARR_PASS;
            }
        }
    }
}