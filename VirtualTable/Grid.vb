
Imports System.Drawing.Imaging

Public Class Grid
   Inherits Disposable
   '''' <summary>Number of rows and columns in the map grid</summary>
   '''' <remarks>Each grid square represents a five foot by five foot area on the map</remarks>
   Public Sub New()
      Me.New(50)
   End Sub
   Public Sub New(pixelsPerSquare As Integer)
      _pixelsPerSquare = pixelsPerSquare
      _gridImage = New Bitmap(_pixelsPerSquare, _pixelsPerSquare)
      Using transparent As Brush = New SolidBrush(Color.Transparent)
         Using line As Pen = New Pen(Color.Gray, 2.0F)
            Using g As Graphics = Graphics.FromImage(_gridImage)
               g.FillRectangle(transparent, 0, 0, _pixelsPerSquare, _pixelsPerSquare)
               g.DrawRectangle(line, 0, 0, _pixelsPerSquare, _pixelsPerSquare)
            End Using
         End Using
      End Using
      _gridBrush = New TextureBrush(_gridImage, Drawing2D.WrapMode.Tile)
   End Sub

   Private _pixelsPerSquare As Integer
   Private _gridImage As Image
   Private _gridBrush As TextureBrush
   Private _gridPixelOrgin As Point

   Public Sub Draw(g As Graphics, srcPixelLocation As Point, destPixelSize As Size)
      Dim destRect As New Rectangle(Point.Empty, destPixelSize)
      _gridPixelOrgin = srcPixelLocation
      _gridBrush.ResetTransform()
      _gridBrush.TranslateTransform(-_gridPixelOrgin.X, -_gridPixelOrgin.Y)
      g.FillRectangle(_gridBrush, destRect)
   End Sub

   Public Function Location(pixelLocation As Point) As Point
      Dim mapPoint As Point = pixelLocation + _gridPixelOrgin
      Return New Point(mapPoint.X \ _pixelsPerSquare, mapPoint.Y \ _pixelsPerSquare)
   End Function

   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(disposing As Boolean)
      Try
         If _gridImage IsNot Nothing Then _gridImage.Dispose()
         If _gridBrush IsNot Nothing Then _gridBrush.Dispose()
         _gridImage = Nothing
         _gridBrush = Nothing
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub
End Class
