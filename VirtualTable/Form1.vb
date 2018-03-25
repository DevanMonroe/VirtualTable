Imports System.Threading

Public Class Form1

   Protected Overrides Sub OnPaint(e As PaintEventArgs)
      Try
         If _map IsNot Nothing Then _map.Draw(e.Graphics, _orgin, Me.ClientSize)
      Catch ex As Exception
         Debug.Print(ex.ToString)
      Finally
         MyBase.OnPaint(e)
      End Try
   End Sub

   Private _map As Map = Nothing
   Private _orgin As Point = Point.Empty
   Private _location As Point = Point.Empty
   Private _scroll As Boolean = False

   Public Sub New()

      ' This call is required by the designer.
      InitializeComponent()

      ' Add any initialization after the InitializeComponent() call.
      _map = New Map With {
         .Size = New Size(800, 1200)
      }
   End Sub

   Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs) Handles Me.MouseDown
      _location = e.Location
      _scroll = True
   End Sub

   Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
      If _scroll Then
         _orgin += _location - e.Location
         _location = e.Location
         Me.Refresh()
      End If
   End Sub

   Private Sub Form1_MouseUp(sender As Object, e As MouseEventArgs) Handles Me.MouseUp
      _scroll = False
   End Sub

   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
         If disposing Then
            If _map IsNot Nothing Then _map.Dispose()
            _map = Nothing
            If components IsNot Nothing Then components.Dispose()
            components = Nothing
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub
End Class



'Private _interlock As Integer
'Private Enum Inter As Integer
'   unlocked = 0
'   locked = 1
'End Enum

'If Interlocked.CompareExchange(_interlock, Inter.locked, Inter.unlocked) = Inter.unlocked Then
'   Try
'   Finally
'      Interlocked.Exchange(_interlock, Inter.unlocked)
'   End Try
'End If