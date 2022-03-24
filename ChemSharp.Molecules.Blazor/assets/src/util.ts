export function getParentSize(canvas: HTMLElement) {
	var box = canvas.parentElement;
	return {width: box.clientWidth, height: box.clientHeight}
}
