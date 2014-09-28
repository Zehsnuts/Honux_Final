var mouseOverColor = Color.blue;
private var originalColor : Color;
function Start () {
	originalColor = renderer.sharedMaterial.color;
}
function OnMouseEnter () {
	renderer.material.color = mouseOverColor;
}

function OnMouseExit () {
	renderer.material.color = originalColor;
}

function OnMouseDown () {
	
}