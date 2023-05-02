<a href="#"><img align="right" src="./etc/logo.png" width="20%"></a>

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://opensource.org/licenses/MIT)

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

See [shader-lsp nuget page]().

## 🔨 Usage

```sh
/path/to/shader-ls --stdio
```

The name is `shader-ls.exe` if you are using Windows!

## 🔧 Settings

- `ShaderLab.CompletionWord` - completing word in the completion service
