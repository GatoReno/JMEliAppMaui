using System.Windows.Input;
using JMEliAppMaui.Services.Abstractions;

namespace JMEliAppMaui.ViewModels
{
    /// <summary>
    /// ViewModel base para cualquier vista que carga datos de Firebase.
    /// Maneja: IsLoading, OnAppearing → LoadDataAsync(), error handling.
    /// 
    /// Uso:
    ///   1. Hereda de DataViewModel
    ///   2. Override LoadDataAsync() con tu lógica de carga
    ///   3. En XAML bindea AppearingCommand al Appearing event del page
    ///   4. Los datos se cargan automáticamente al aparecer la vista
    /// </summary>
    public abstract class DataViewModel : BindableObject
    {
        #region Shared State

        private bool _isLoading;
        private bool _hasError;
        private string? _errorMessage;
        private bool _isEmpty;
        private bool _isDataLoaded;

        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsNotLoading)); } }
        public bool IsNotLoading => !IsLoading;
        public bool HasError { get => _hasError; set { _hasError = value; OnPropertyChanged(); } }
        public string? ErrorMessage { get => _errorMessage; set { _errorMessage = value; OnPropertyChanged(); } }
        public bool IsEmpty { get => _isEmpty; set { _isEmpty = value; OnPropertyChanged(); } }

        /// <summary>If true, data was already loaded — skip re-fetch on next OnAppearing unless ForceReload.</summary>
        public bool IsDataLoaded { get => _isDataLoaded; set { _isDataLoaded = value; OnPropertyChanged(); } }

        #endregion

        #region Commands

        public ICommand AppearingCommand { get; }
        public ICommand RefreshCommand { get; }

        #endregion

        protected readonly IFirebaseService Firebase;

        protected DataViewModel(IFirebaseService firebase)
        {
            Firebase = firebase;
            AppearingCommand = new Command(async () => await OnAppearingAsync());
            RefreshCommand = new Command(async () => await ForceReloadAsync());

            // Trigger initial load (non-blocking) — covers cases where OnAppearing isn't called
            MainThread.BeginInvokeOnMainThread(async () => await OnAppearingAsync());
        }

        /// <summary>
        /// Called on every OnAppearing. Only loads data if not already loaded.
        /// Override LoadDataAsync() for your specific data fetching.
        /// </summary>
        private async Task OnAppearingAsync()
        {
            if (IsDataLoaded && !ShouldReloadOnAppearing())
                return;

            await SafeLoadAsync();
        }

        /// <summary>Force reload — ignores IsDataLoaded flag. Use for pull-to-refresh.</summary>
        private async Task ForceReloadAsync()
        {
            IsDataLoaded = false;
            await SafeLoadAsync();
        }

        /// <summary>
        /// Wraps LoadDataAsync with loading state + error handling.
        /// </summary>
        private async Task SafeLoadAsync()
        {
            try
            {
                IsLoading = true;
                HasError = false;
                ErrorMessage = null;

                await LoadDataAsync();

                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Error al cargar datos: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"[{GetType().Name}] LoadData error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Override this to load your specific data.
        /// Called automatically on OnAppearing (first time) and on pull-to-refresh.
        /// Use Firebase.GetAllAsync / GetWhereAsync here.
        /// </summary>
        protected abstract Task LoadDataAsync();

        /// <summary>
        /// Override to return true if you always want fresh data on every OnAppearing.
        /// Default: false (loads once, then uses cached state).
        /// </summary>
        protected virtual bool ShouldReloadOnAppearing() => false;

        /// <summary>
        /// Call this when you know the data changed (e.g. after adding an item)
        /// to force the next OnAppearing to reload.
        /// </summary>
        protected void InvalidateData() => IsDataLoaded = false;
    }
}
