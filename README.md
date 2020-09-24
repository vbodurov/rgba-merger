# rgba-merger
Command line tool to merge Red Green Blue Alpha channel into a single image

Parameters:
 * -r - red file path or byte number 0 - 255
 * -g - green file path or byte number 0 - 255
 * -b - blue file path or byte number 0 - 255
 * -a - alpha file path or byte number 0 - 255
 * -o - output image path (must end with .png)

At least one input image is required. 

Output image path is required.

Width and Height for all images must be the same.

Sample calls:

```bash
.\rgbamerge -r C:\Temp\r.jpg -g C:\Temp\g.jpg -b C:\Temp\b.jpg -a C:\Temp\a.jpg -o C:\Temp\o.png
```

```bash
.\rgbamerge -r "C:\Temp\r.jpg" -g 255 -b 0 -a "C:\Temp\a.jpg" -o "C:\Temp\o.png"
```
