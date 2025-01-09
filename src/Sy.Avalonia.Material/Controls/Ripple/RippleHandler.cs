using Avalonia.Media;
using Avalonia.Rendering.Composition;

namespace Sy.Avalonia.Material.Controls;

internal sealed class RippleHandler : CompositionCustomVisualHandler {
	public static readonly object StartSpreadMessage = new();
	public static readonly object StartFadeMessage = new();

	private readonly RippleInfo _info;
	private TimeSpan _animationElapsed;
	private TimeSpan? _lastServerTime;
	private TimeSpan? _fadeStartedAt;

	public RippleHandler(RippleInfo info) {
		_info = info;
	}

	public override void OnAnimationFrameUpdate() {
		if (_fadeStartedAt is not null) {
			var fadeElapsed = _animationElapsed - _fadeStartedAt.Value;
			if (fadeElapsed > _info.FadeDuration) return;
		}
		Invalidate();
		RegisterForNextAnimationFrameUpdate();
	}

	public override void OnMessage(object message) {
		if (message == StartSpreadMessage) {
			_lastServerTime = null;
			_fadeStartedAt = null;
			RegisterForNextAnimationFrameUpdate();
		} else if (message == StartFadeMessage) {
			_fadeStartedAt = _animationElapsed;
		}
	}

	public override void OnRender(ImmediateDrawingContext drawingContext) {
		if (_lastServerTime is not null) {
			_animationElapsed += CompositionNow - _lastServerTime.Value;
		}
		_lastServerTime = CompositionNow;

		var currentRadius = CalculateCurrentRadius(_animationElapsed, _fadeStartedAt);
		var currentOpacity = CalculateCurrentOpacity(_animationElapsed, _fadeStartedAt);
		PushValues(drawingContext, currentOpacity, currentRadius);
	}

	private double CalculateCurrentRadius(TimeSpan totalElapsed, TimeSpan? fadeStartedAt) {
		var percentRaw = totalElapsed / _info.SpreadDuration;
		if (fadeStartedAt is not null && percentRaw < 1d) {
			var fadeDuration = _info.FadeDuration;
			var fadeElapsed = totalElapsed - fadeStartedAt.Value;
			var fadePercent = fadeElapsed / fadeDuration;

			var percentRemaining = 1 - percentRaw;
			percentRaw += percentRemaining * fadePercent;
		}
		if (percentRaw > 1d) percentRaw = 1d;

		var percentEased = _info.Easing.Ease(percentRaw);
		var current = _info.MaxRadius * percentEased;
		return current;
	}

	private double CalculateCurrentOpacity(TimeSpan totalElapsed, TimeSpan? fadeStartedAt) {
		var maxOpacity = _info.Opacity;
		if (fadeStartedAt is null) return maxOpacity;

		var duration = _info.FadeDuration;
		var easing = _info.Easing;

		var elapsed = totalElapsed - fadeStartedAt.Value;
		var percentRaw = elapsed / duration;
		if (percentRaw > 1d) percentRaw = 1d;

		var percentEased = easing.Ease(percentRaw);
		var current = maxOpacity - maxOpacity * percentEased;

		return current;
	}

	private void PushValues(ImmediateDrawingContext context, double opacity, double radius) {
		using var clipState = context.PushClip(_info.Clip);
		using var opacityState = context.PushOpacity(opacity, default);
		context.DrawEllipse(_info.Brush, null, _info.Origin, radius, radius);
	}
}
