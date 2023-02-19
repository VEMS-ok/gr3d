# Contributing Guidelines

## Coding practices
- The project follows coding conventions outlined by [MRTK](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/contributing/coding-guidelines?view=mrtkunity-2022-05). Please follow them where applicable. Also refer to the [Microsoft coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions), which is another wonderful resource.
- Provide documentation to all public fields and classes. This project uses [Doxygen](https://www.doxygen.nl/index.html) to generate documentation.
  - If you are including examples in the documentation, you may want to use the doxygen's [code command](https://www.doxygen.nl/manual/commands.html#cmdcode).
- Docuement all Prefabs, what their purpose is and how they can be used.
- Ensure to add appropriate entries to `CHANGELOG.md`
- All code is nested in the namespace `ubc.ok.VEMS`.
  - `ubc.ok.VEMS.gr3d` - Components that depend on getReal3D is in the namespace.
  - `ubc.ok.VEMS.Utils` - General components that don't depend on the getReal3D
  
  
## Generating documentation
The project uses [Doxygen](https://www.doxygen.nl/index.html) to generate the documentation. You'll need to have [doxygen](https://www.doxygen.nl/download.html) installed on your system (either have it added to the `PATH` variable or use the full path to the installation directory)

From your terminal (or PowerShell or command prompt) run doxygen:
```sh
doxygen
```

This will generate the documentation. You can push the changes and additions to the `docs` folder to GitHub.
Once merged into the main branch, it will generate the website which can be seen at https://vems-ok.github.io/gr3d/

## Generating a new release
First, update the `CHANGELOG.md` with the version and accompanying changes (see [semantic versioning](https://semver.org) for more information on versioning).
From https://github.com/VEMS-ok/gr3d/releases select draft a new release. Provide the information to create a new release:
- Select `Choose a tag` and provide the new version as the tag.
- Add appropriate descriptions. Ideally, this includes the content of the changelog of this version. 

Once you select publish, a new unitypackage will be [generated and attached to the release](https://github.com/VEMS-ok/gr3d/blob/master/.github/workflows/package_release.yml).
