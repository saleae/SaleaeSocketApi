Starting January 4 2022, Saleae has released a new hardware revision of our Logic Pro 16 devices. These new units are not compatible with the legacy Logic 1.x software, and require the latest version of the Logic 2 software, available [here](https://www.saleae.com/downloads/).
We are working toward a solution for automation users who are still using the Logic 1.x software. If you have already purchased new Logic Pro 16 devices after the date mentioned above, or plan to order new units for use with the Logic 1.x automation API, please contact us for details [here](https://contact.saleae.com/hc/en-us/requests/new).
Users can check their hardware revision with the instructions [here](https://support.saleae.com//faq/technical-faq/how-to-find-your-devices-hardware-revision).

# Saleae Socket API C# Wrapper and Example Projects

This example was created with Visual Studio 2013 and targets the Microsoft .NET 4.5 runtime.
Generally the head of this repo will require the latest Saleae beta. At request, we can also add tags for specific version support.
This repo was created alongside the release of the 1.2.5 beta software. Support for older versions is available upon request. The 1.1.18 software was the first major version to include socket API support, and many of the functions have remained unchanged.

The Saleae Logic software includes a TCP socket server which can be used by 3rd party applications to automate common software actions, such as starting and saving captures, changing capture settings, exporting raw data, protocol data, etc.

This does not provide real time access to the raw data from the device, and requires the Saleae Logic desktop application to be running.

## Common applications of this API:
- Breaking long captures into a series of shorter captures, saved back to back in order to reduce memory consumption and allow for significantly loner recordings.
- Automatically capture and export either raw data or protocol data in automated QA environments, such as production line unit testing, etc.
- OEM integration into other products as a data acquisition device.

## Application Description
The included visual studio solution includes at least two projects:
- SaleaeSocketApi
- SaleaeSocketApiExample

The SaleaeSocketApi is a class library that can be easily re-used in other .NET projects, and wraps all of the Logic Software API commands. The main class to consume is SaleaeClient.
The SaleaeSocketApiExample is a simple console application that demonstrates the SaleaeSocketApi's main class, SaleaeClient. It will dump information, change settings and start a capture.
Other projects might be included, which are likely used for internal testing at Saleae, but are provided as examples. They probably don't completely work.

Documentation can be found in the Doc folder. Feel free to fork and send pull requests, if you find any bugs!
