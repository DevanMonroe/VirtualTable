Public Class Disposable
   Implements IDisposable

   Private _isDisposed As Boolean

   <System.Diagnostics.DebuggerNonUserCode()>
   Public ReadOnly Property IsDisposed
      Get
         Return _isDisposed
      End Get
   End Property

   <System.Diagnostics.DebuggerNonUserCode()>
   Public Sub RaiseExceptionIfDisposed()
      If _isDisposed Then Throw New ObjectDisposedException(Me.GetType.Name)
   End Sub

   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overridable Sub Dispose(disposing As Boolean)
      _isDisposed = True
   End Sub

   <System.Diagnostics.DebuggerNonUserCode()>
   Protected Overrides Sub Finalize()
      Try
         Try
            Dispose(False)
         Finally
            MyBase.Finalize()
         End Try
         If Not _isDisposed Then Debug.Print("Finalize failure {0}", Me.GetType.Name)
      Catch

      End Try
   End Sub

   <System.Diagnostics.DebuggerNonUserCode()>
   Public Sub Dispose() Implements IDisposable.Dispose
      Try
         Try
            Dispose(True)
         Finally
            GC.SuppressFinalize(Me)
         End Try
         If Not _isDisposed Then Debug.Print("Dispose failure {0}", Me.GetType.Name)
      Catch

      End Try
   End Sub
End Class
