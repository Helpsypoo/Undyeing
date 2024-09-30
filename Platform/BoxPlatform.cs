using Godot;

[Tool]
public partial class BoxPlatform : Platform
{
    private MeshInstance3D MeshInstance3D => GetNode<MeshInstance3D>("MeshInstance3D");
    private CollisionShape3D CollisionShape3D => GetNode<CollisionShape3D>("CollisionShape3D");
    private Vector3 _dimensions = Vector3.One;

    [Export]
    private Vector3 Dimensions
    {
        get => _dimensions;
        set
        {
            _dimensions = value;

            MeshInstance3D.Position = new Vector3(0, -_dimensions.Y / 2, 0);
            CollisionShape3D.Position = new Vector3(0, -_dimensions.Y / 2, 0);

            var mesh = new BoxMesh(); 
            mesh.Size = _dimensions;
            MeshInstance3D.Mesh = mesh;
                
            var shape = new BoxShape3D(); 
            shape.Size = _dimensions;
            CollisionShape3D.Shape = shape;
        }
    }  
    
}