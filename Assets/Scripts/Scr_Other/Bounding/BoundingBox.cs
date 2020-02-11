public class Boundary
{
    public float xMin { get; set; }
    public float xMax { get; set; }
    public float zMin { get; set; }
    public float zMax { get; set; }
}

public class BoundarySettings
{
    public void SetBoundary(Boundary boundary, Resolution resolution)
    {
        if (resolution == Resolution.R_16_9) Resolution_R_16_9(boundary);
    }

    private void Resolution_R_16_9(Boundary boundary)
    {
        boundary.xMin = -68;
        boundary.xMax = 68;
        boundary.zMin = -35;
        boundary.zMax = 35;
    }
}