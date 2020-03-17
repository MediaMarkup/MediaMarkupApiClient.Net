
# .Net client library for the MediaMarkup File approval API.


## Changes

This library is still in development nad subject to change a fair amount until version 1.0.0.0.

## Breaking Changes 

### 1.0.0-CI00001 Preview Changes

1. In [UserCreateParameters](https://github.com/MediaMarkup/MediaMarkupApiClient.Net/blob/master/src/MediaMarkup/Api/Models/UserCreateParameters.cs#L51) payload `UserRole` property is renamed to `Role`
1. `AddApprovalGroupUser` method is renamed to [UpsertApprovalGroupUser](https://github.com/MediaMarkup/MediaMarkupApiClient.Net/blob/master/src/MediaMarkup/Api/IApprovals.cs#L128)
1. [ApprovalGroupUserParameters](https://github.com/MediaMarkup/MediaMarkupApiClient.Net/blob/master/src/MediaMarkup/Api/Models/ApprovalGroupUserParameters.cs) payload doesn't support `AllowVersionSelection` parameter in this version. **It is under development.**
1. `Authenticated` method call and requirement is now obsolete

## MediaMarkup API

### API Endpoint & Manual testing

All end-points can be found and used interatively at [https://www.mediamarkup.com/api-docs/](https://www.mediamarkup.com/api-docs/)

(under development, probbaly the most useful resource at present)

### Full Documentation

Full documentation on how to use this library can be found at [https://mediamarkup.gitbook.io/docs](https://mediamarkup.gitbook.io/docs)

### Installation

```
Install-Package MediaMarkupApiClient
```

### Usage

API Keys are available on your account page after signing up.  

...  

## Issue Reporting

If you have found a bug or if you have a feature request, please report them at this repository issues section. 
Please do not report security vulnerabilities on the public GitHub issue tracker. please disclose any security issues directly to us at support@mediamarkup.com

## Author

[Brighter Tools Ltd](www.brightertools.com)

## License

This project is licensed under the MIT license. See the LICENSE.txt file.
