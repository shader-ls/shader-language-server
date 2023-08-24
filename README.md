<a href="#"><img align="right" src="https://raw.githubusercontent.com/shader-ls/shader-language-server/master/etc/logo.png" width="20%"></a>

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)
[![Release](https://img.shields.io/github/tag/shader-ls/shader-language-server.svg?label=release&logo=github)](https://github.com/shader-ls/shader-language-server/releases/latest)
[![Nuget DT](https://img.shields.io/nuget/dt/shader-ls?logo=nuget&logoColor=49A2E6)](https://www.nuget.org/packages/shader-ls/)

# shader-language-server
> Language server implementation for ShaderLab

## 🚧 Status

Still in development.

#### Current Features

- Completion
- Hover
- Signature Help

#### Planned Features

- Diagnostics
- Jump to def

## 💾 Installation

`dotnet tool install --global shader-ls`

See [shader-lsp nuget page](https://www.nuget.org/packages/shader-ls/).

## 🔨 Usage

```sh
/path/to/shader-ls --stdio
```

The name is `shader-ls.exe` if you are using Windows!

## 🔧 Settings

- `ShaderLab.CompletionWord` - completing word in the completion service
