<p align="center">
  <!-- <a href="#"><img align="right" src="https://raw.githubusercontent.com/shader-ls/shader-language-server/master/etc/logo.png" width="20%"></a> -->
  <a href="#"><img src="./etc/logo.png" width="20%"></a>
  <h1 align="center">Shader Language Server</h1>
  <p align="center">
    <a href="https://opensource.org/licenses/MIT"><img alt="License" src="https://img.shields.io/badge/License-MIT-green.svg"/></a>
    <a href="https://github.com/shader-ls/shader-language-server/releases/latest"><img alt="Release" src="https://img.shields.io/github/tag/shader-ls/shader-language-server.svg?label=release&logo=github"/></a>
    <a href="https://www.nuget.org/packages/shader-ls/"><img alt="Nuget DT" src="https://img.shields.io/nuget/dt/shader-ls?logo=nuget&logoColor=49A2E6"/></a>
  </p>
  <p align="center">
    <a href="https://github.com/shader-ls/shader-language-server/actions/workflows/test.yml"><img alt="License" src="https://github.com/shader-ls/shader-language-server/actions/workflows/test.yml/badge.svg"/></a>
  </p>
</p>

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
