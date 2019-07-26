
namespace BuildReportTool
{

// per platform identification
// needed to handle special cases
// example: some platforms have a compressed build, some do not.
// also, native plugins are handled differently in each platform.
public enum BuildPlatform
{
	None = 0,

	// -------
	// Mobiles
	// -------

	Android = 1,
	iOS,
	Blackberry,
	WindowsPhone8,
	Tizen,


	// --------
	// Web
	// --------

	Web = 100,
	Flash,
	WebGL, // upcoming

	
	// --------
	// Desktops
	// --------

	// distinctions between 32 or 64 bit need to be made to
	// determine which existing native plugins are used or not

	MacOSX32 = 200,
	MacOSX64,
	MacOSXUniversal,

	Windows32 = 300,
	Windows64,

	Linux32 = 400,
	Linux64,
	LinuxUniversal,


	// ------
	// Consoles (7th gen)
	// ------

	// currently not handled in any special way (probably needs to be):
	XBOX360 = 500,
	PS3,
	Wii, // for posterity


	// ------
	// Consoles (8th gen)
	// ------
	
	XBOXOne = 600,
	
	PS4,
	PSVitaNative,
	PSMobile,
	
	WiiU,
	Nintendo3DS
}

}
