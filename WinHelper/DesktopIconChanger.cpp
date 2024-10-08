#include <ShlObj.h>     // Shell API
#include <atlcomcli.h>  // CComPtr & Co.
#include <string> 
#include <iostream> 
#include <system_error>
#include "DesktopIconChanger.h"

// Throw a std::system_error if the HRESULT indicates failure.
template< typename T >
void ThrowIfFailed(HRESULT hr, T&& msg)
{
    if (FAILED(hr))
        throw std::system_error{ hr, std::system_category(), std::forward<T>(msg) };
}

// RAII wrapper to initialize/uninitialize COM
struct CComInit
{
    HRESULT hr = ::CoInitialize(nullptr);
    CComInit() { ThrowIfFailed(hr, "CoInitialize failed"); }
    ~CComInit() { ::CoUninitialize(); }
};

// Query an interface from the desktop shell view.
void FindDesktopFolderView(REFIID riid, void** ppv, std::string const& interfaceName)
{
    CComPtr<IShellWindows> spShellWindows;
    ThrowIfFailed(
        spShellWindows.CoCreateInstance(CLSID_ShellWindows),
        "Failed to create IShellWindows instance");

    CComVariant vtLoc(CSIDL_DESKTOP);
    CComVariant vtEmpty;
    long lhwnd;
    CComPtr<IDispatch> spdisp;
    ThrowIfFailed(
        spShellWindows->FindWindowSW(
            &vtLoc, &vtEmpty, SWC_DESKTOP, &lhwnd, SWFO_NEEDDISPATCH, &spdisp),
        "Failed to find desktop window");

    CComQIPtr<IServiceProvider> spProv(spdisp);
    if (!spProv)
        ThrowIfFailed(E_NOINTERFACE, "Failed to get IServiceProvider interface for desktop");

    CComPtr<IShellBrowser> spBrowser;
    ThrowIfFailed(
        spProv->QueryService(SID_STopLevelBrowser, IID_PPV_ARGS(&spBrowser)),
        "Failed to get IShellBrowser for desktop");

    CComPtr<IShellView> spView;
    ThrowIfFailed(
        spBrowser->QueryActiveShellView(&spView),
        "Failed to query IShellView for desktop");

    ThrowIfFailed(
        spView->QueryInterface(riid, ppv),
        "Could not query desktop IShellView for interface " + interfaceName);
}

int DesktopIconChanger::SetDesktopIconSize(int size)
{
    try
    {
        CComInit coInit;

        CComPtr<IFolderView2> spView;
        FindDesktopFolderView(IID_PPV_ARGS(&spView), "IFolderView2");

        FOLDERVIEWMODE viewMode = FVM_AUTO;
        //int iconSize = 0;
        //ThrowIfFailed(
        //    spView->GetViewModeAndIconSize(&viewMode, &iconSize),
        //    "GetViewModeAndIconSize failed");
        //std::cout << "Current view mode: " << viewMode << ", icon size: " << iconSize << '\n';

        ThrowIfFailed(
            spView->SetViewModeAndIconSize(viewMode, size),
            "SetViewModeAndIconSize failed");

        return 0;
    }
    catch (std::system_error const& e)
    {
        std::cout << "ERROR: " << e.what() << ", error code: " << e.code() << "\n";
        return 1;
    }
}

int DesktopIconChanger::SetDesktopIconMode(int mode)
{
    try
    {
        FOLDERVIEWMODE viewMode = FVM_AUTO;
        if (mode >= 0 && mode < 10)
        {
            viewMode = FVM_AUTO;
        }
        else if (mode >= 10 && mode < 20)
        {
            viewMode = FVM_FIRST;
        }
        else if (mode >= 20 && mode < 30)
        {
            viewMode = FVM_SMALLICON;
        }
        else if (mode >= 30 && mode < 40)
        {
            viewMode = FVM_LIST;
        }
        else if (mode >= 40 && mode < 50)
        {
            viewMode = FVM_DETAILS;
        }
        else if (mode >= 50 && mode < 60)
        {
            viewMode = FVM_THUMBNAIL;
        }
        else if (mode >= 60 && mode < 70)
        {
            viewMode = FVM_TILE;
        }
        else if (mode >= 70 && mode < 80)
        {
            viewMode = FVM_THUMBSTRIP;
        }
        else if (mode >= 80 && mode < 90)
        {
            viewMode = FVM_CONTENT;
        }
        else if (mode >= 90 && mode <= 100)
        {
            viewMode = FVM_CONTENT;
        }

        CComInit coInit;

        CComPtr<IFolderView2> spView;
        FindDesktopFolderView(IID_PPV_ARGS(&spView), "IFolderView2");

        int iconSize = 0;
        FOLDERVIEWMODE viewModeCache = FVM_AUTO;
        ThrowIfFailed(
            spView->GetViewModeAndIconSize(&viewModeCache, &iconSize),
            "GetViewModeAndIconSize failed");

        ThrowIfFailed(
            spView->SetViewModeAndIconSize(viewMode, iconSize),
            "SetViewModeAndIconSize failed");

        return 0;
    }
    catch (std::system_error const& e)
    {
        std::cout << "ERROR: " << e.what() << ", error code: " << e.code() << "\n";
        return 1;
    }
}