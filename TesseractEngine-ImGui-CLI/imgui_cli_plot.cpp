#include "implot.h"
#include "imgui_cli.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Numerics;


namespace Tesseract { namespace CLI { namespace ImGui { namespace Addon {

	public enum class ImAxis {
		X1 = ImAxis_X1,
		X2 = ImAxis_X2,
		X3 = ImAxis_X3,
		Y1 = ImAxis_Y1,
		Y2 = ImAxis_Y2,
		Y3 = ImAxis_Y3
	};

	[FlagsAttribute]
	public enum class ImPlotFlags {
		None = 0,
		/// <summary>
		/// The plot title will not be displayed.
		/// </summary>
		NoTitle = ImPlotFlags_NoTitle,
		/// <summary>
		/// The legend will not be displayed.
		/// </summary>
		NoLegend = ImPlotFlags_NoLegend,
		/// <summary>
		/// The position within the plot will not be displayed at the mouse cursor.
		/// </summary>
		NoMouseText = ImPlotFlags_NoMouseText,
		/// <summary>
		/// The user will not be able to interact with the plot.
		/// </summary>
		NoInputs = ImPlotFlags_NoInputs,
		/// <summary>
		/// The user cannot open context menus on the plot.
		/// </summary>
		NoMenus = ImPlotFlags_NoMenus,
		/// <summary>
		/// The user cannot select a box within the plot.
		/// </summary>
		NoBoxSelect = ImPlotFlags_NoBoxSelect,
		/// <summary>
		/// A child window will not be used to capture mouse scrolling.
		/// </summary>
		NoChild = ImPlotFlags_NoChild,
		/// <summary>
		/// The frame will not be rendered.
		/// </summary>
		NoFrame = ImPlotFlags_NoFrame,
		/// <summary>
		/// X and Y axis pairs will have the same units per pixel.
		/// </summary>
		Equal = ImPlotFlags_Equal,
		/// <summary>
		/// The cursor will be a crosshair when hovered over the plot.
		/// </summary>
		Crosshairs = ImPlotFlags_Crosshairs,
		/// <summary>
		/// Only the plotting canvas will be displayed.
		/// </summary>
		CanvasOnly = ImPlotFlags_CanvasOnly
	};

	[FlagsAttribute]
	public enum class ImPlotAxisFlags {
		None = 0,
		/// <summary>
		/// The axis label will not be displayed.
		/// </summary>
		NoLabel = ImPlotAxisFlags_NoLabel,
		/// <summary>
		/// No grid lines will be displayed.
		/// </summary>
		NoGridLines = ImPlotAxisFlags_NoGridLines,
		/// <summary>
		/// No tick marks will be displayed.
		/// </summary>
		NoTickMarks = ImPlotAxisFlags_NoTickMarks,
		/// <summary>
		/// No text labels will be displayed.
		/// </summary>
		NoTickLabels = ImPlotAxisFlags_NoTickLabels,
		/// <summary>
		/// The axis will not be initially fit to the displayed data on the first frame.
		/// </summary>
		NoInitialFit = ImPlotAxisFlags_NoInitialFit,
		/// <summary>
		/// The user cannot open context menus on the axis.
		/// </summary>
		NoMenus = ImPlotAxisFlags_NoMenus,
		/// <summary>
		/// The user cannot switch the axis side by dragging it.
		/// </summary>
		NoSideSwitch = ImPlotAxisFlags_NoSideSwitch,
		/// <summary>
		/// The axis will not have its background highlighted when hovered or held.
		/// </summary>
		NoHighlight = ImPlotAxisFlags_NoHighlight,
		/// <summary>
		/// Axis ticks and labels will be rendered on the conventionally opposite side (right or top).
		/// </summary>
		Opposite = ImPlotAxisFlags_Opposite,
		/// <summary>
		/// Grid lines will be displayed in the foreground in front of the data instead of the background.
		/// </summary>
		Foreground = ImPlotAxisFlags_Foreground,
		/// <summary>
		/// The axis will be inverted.
		/// </summary>
		Invert = ImPlotAxisFlags_Invert,
		/// <summary>
		/// The axis will automatically fit to the displayed data.
		/// </summary>
		AutoFit = ImPlotAxisFlags_AutoFit,
		/// <summary>
		/// The axis will only fit points if within the range of the orthogonal axis.
		/// </summary>
		RangeFit = ImPlotAxisFlags_RangeFit,
		/// <summary>
		/// Panning while in a locked state will cause the axis to stretch if possible.
		/// </summary>
		PanStretch = ImPlotAxisFlags_PanStretch,
		/// <summary>
		/// The axis minimum value will be locked when panning or zooming.
		/// </summary>
		LockMin = ImPlotAxisFlags_LockMin,
		/// <summary>
		/// The axis maximum value will be locked when panning or zooming.
		/// </summary>
		LockMax = ImPlotAxisFlags_LockMax,
		/// <summary>
		/// The axis will be locked when panning or zooming.
		/// </summary>
		Lock = ImPlotAxisFlags_Lock,
		/// <summary>
		/// No decorations will be applied to the axis.
		/// </summary>
		NoDecorations = ImPlotAxisFlags_NoDecorations,
		/// <summary>
		/// An auxiliary default value for axis flags.
		/// </summary>
		AuxDefault = ImPlotAxisFlags_AuxDefault
	};

	[FlagsAttribute]
	public enum class ImPlotSubplotFlags {
		None = 0,
		/// <summary>
		/// The subplot title will not be displayed.
		/// </summary>
		NoTitle = ImPlotSubplotFlags_NoTitle,
		/// <summary>
		/// The legend will not be displayed.
		/// </summary>
		NoLegend = ImPlotSubplotFlags_NoLegend,
		/// <summary>
		/// The user cannot open context menus on this subplot.
		/// </summary>
		NoMenus = ImPlotSubplotFlags_NoMenus,
		/// <summary>
		/// The subplot cannot be resized.
		/// </summary>
		NoResize = ImPlotSubplotFlags_NoResize,
		/// <summary>
		/// The edges of the subplot will not be automatically horizontally or vertically aligned.
		/// </summary>
		NoAlign = ImPlotSubplotFlags_NoAlign,
		/// <summary>
		/// Items across subplots will be shared and rendered in a single legend.
		/// </summary>
		ShareItems = ImPlotSubplotFlags_ShareItems,
		/// <summary>
		/// Links the Y axis limits of all plots in the row.
		/// </summary>
		LinkRows = ImPlotSubplotFlags_LinkRows,
		/// <summary>
		/// Links the X axis limits of all plots in the column.
		/// </summary>
		LinkCols = ImPlotSubplotFlags_LinkCols,
		/// <summary>
		/// Links the Y axis limits of every plot in the subplot.
		/// </summary>
		LinkAllX = ImPlotSubplotFlags_LinkAllX,
		/// <summary>
		/// Links the X axis limits of every plot in the subplot.
		/// </summary>
		LinkAllY = ImPlotSubplotFlags_LinkAllY,
		/// <summary>
		/// Subplots are added in column-major order instead of row-major.
		/// </summary>
		ColMajor = ImPlotSubplotFlags_ColMajor
	};

	[FlagsAttribute]
	public enum class ImPlotLegendFlags {
		None,
		/// <summary>
		/// Legend icons will not function as hide/show buttons.
		/// </summary>
		NoButtons,
		/// <summary>
		/// Plot items will not be highlighted when their legend entry is hovered.
		/// </summary>
		NoHighlightItem,
		/// <summary>
		/// Axes will not be highlighted when their legend entry is hovered.
		/// </summary>
		NoHighlightAxis,
		/// <summary>
		/// The user cannot open context menus on this legend.
		/// </summary>
		NoMenus,
		/// <summary>
		/// The legend will be rendered outside the plot area.
		/// </summary>
		Outside,
		/// <summary>
		/// The legend entries will be displayed horizontally.
		/// </summary>
		Horizontal,
		/// <summary>
		/// The legend entries will be sorted in alphabetical order.
		/// </summary>
		Sort
	};

	[FlagsAttribute]
	public enum class ImPlotMouseTextFlags {
		None,
		/// <summary>
		/// Only show the mouse position for primary axes.
		/// </summary>
		NoAuxAxes,
		/// <summary>
		/// Axes label formatters won't be used to render text.
		/// </summary>
		NoFormat,
		/// <summary>
		/// Always display the mouse position even if a plot is not hovered.
		/// </summary>
		ShowAlways
	};

	[FlagsAttribute]
	public enum class ImPlotDragToolFlags {
		None,
		/// <summary>
		/// Drag tools won't change cursor icons when hovered or held.
		/// </summary>
		NoCursors,
		/// <summary>
		/// The drag tool won't be considered for plot fits.
		/// </summary>
		NoFit,
		/// <summary>
		/// Lock the tool from user inputs.
		/// </summary>
		NoInputs,
		/// <summary>
		/// Tool rendering will be delayed one frame.
		/// </summary>
		Delayed
	};

	[FlagsAttribute]
	public enum class ImPlotColormapScaleFlags {
		None,
		/// <summary>
		/// The colormap axis label will not be displayed.
		/// </summary>
		NoLabel = ImPlotColormapScaleFlags_NoLabel,
		/// <summary>
		/// The colormap label and tick labels will render on the opposite side.
		/// </summary>
		Opposite = ImPlotColormapScaleFlags_Opposite,
		/// <summary>
		/// Inverts the colormap bar and axis scale.
		/// </summary>
		Invert = ImPlotColormapScaleFlags_Invert
	};

	public value struct ImPlotStyle {
	public:
		float LineWeigth;
		int Marker;
		float MarkerSize;
		float MarkerWeight;
		float FillAlpha;
		float ErrorBarSize;
		float ErrorBarWeight;
		float DigitalBitHeight;
		float DigitalBitGap;
		float PlotBorderSize;
		float MinorAlpha;
		Vector2 MajorTickLen;
		Vector2 MinorTickLen;
		Vector2 MajorTickSize;
		Vector2 MinorTickSize;
		Vector2 MajorGridSize;
		Vector2 MinorGridSize;
		Vector2 PlotPadding;
		Vector2 LabelPadding;
		Vector2 LegendPadding;
		Vector2 LegendInnerPadding;
		Vector2 LegendSpacing;
		Vector2 MousePosPadding;
		Vector2 AnnotationPadding;
		Vector2 FitPadding;
		Vector2 PlotDefaultSize;
		Vector2 PlotMinSize;

	};

	public ref class ImPlot abstract sealed {
	public:

	};

}}}}