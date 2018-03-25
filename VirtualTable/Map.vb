Public Class Map
   Inherits Disposable

   Private _background As New Background
   Private _grid As New Grid

   Public Property Background As Image
      Get
         Return _background.Image
      End Get
      Set(value As Image)
         _background.Image = value
      End Set
   End Property

   Public Property Size As Size
      Get
         Return _background.Size
      End Get
      Set(value As Size)
         _background.Size = value
      End Set
   End Property

   Public Sub Draw(ByVal g As Graphics, ByVal srcLocation As Point, ByVal destSize As Size)
      destSize = ConstrainSize(destSize)
      srcLocation = ConstrainLocation(srcLocation, destSize)
      _background.Draw(g, srcLocation, destSize)
      _grid.Draw(g, srcLocation, destSize)
   End Sub

   Private Function ConstrainSize(destSize As Size) As Size
      Dim rectMap As New Rectangle(Point.Empty, _background.Size)
      Dim rectDest As New Rectangle(Point.Empty, destSize)
      rectDest.Intersect(rectMap)
      Return rectDest.Size
   End Function

   Private Function ConstrainLocation(ByVal srcLocation As Point, ByVal destSize As Size) As Point
      Static border As Size = New Size(1, 1)
      Dim rect As New Rectangle(Point.Empty, _background.Size - destSize - border)
      If rect.Contains(srcLocation) Then Return srcLocation
      Dim x As Integer = Math.Max(0, Math.Min(rect.Width, srcLocation.X))
      Dim y As Integer = Math.Max(0, Math.Min(rect.Height, srcLocation.Y))
      Return New Point(x, y)
   End Function

   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(disposing As Boolean)
      Try
         If _background IsNot Nothing Then _background.Dispose()
         If _grid IsNot Nothing Then _grid.Dispose()
         _background = Nothing
         _grid = Nothing
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub
End Class
