# Contributing to Visual Novel Framework

Thank you for your interest in contributing! This document provides guidelines for contributing to this project.

## How to Contribute

### Reporting Bugs

If you find a bug, please create an issue with:
- Clear description of the problem
- Steps to reproduce
- Expected behavior
- Actual behavior
- Unity version
- Screenshots if applicable

### Suggesting Features

Feature requests are welcome! Please:
- Check if the feature already exists
- Provide clear use case
- Explain why it would be useful
- Provide examples if possible

### Submitting Code

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes**
4. **Test thoroughly**
5. **Commit with clear messages**
   ```bash
   git commit -m "Add feature: description"
   ```
6. **Push to your fork**
   ```bash
   git push origin feature/your-feature-name
   ```
7. **Create a Pull Request**

## Code Standards

### C# Style Guide

- Use PascalCase for class names and public methods
- Use camelCase for private fields and local variables
- Use UPPERCASE for constants
- Add XML documentation comments for public APIs
- Keep methods focused and concise
- Use regions to organize code sections

Example:
```csharp
/// <summary>
/// Manages character sprites and animations
/// </summary>
public class CharacterManager : MonoBehaviour
{
    private const float DEFAULT_TRANSITION_SPEED = 1.0f;
    
    [SerializeField]
    private Transform characterContainer;
    
    /// <summary>
    /// Shows a character on screen
    /// </summary>
    /// <param name="characterName">Name of the character</param>
    public void ShowCharacter(string characterName)
    {
        // Implementation
    }
}
```

### Unity Best Practices

- Use SerializeField for inspector-visible private fields
- Cache component references in Awake/Start
- Use object pooling for frequently created objects
- Implement proper cleanup in OnDestroy
- Use coroutines for time-based operations
- Follow Unity naming conventions

### Namespaces

All scripts should use appropriate namespaces:
- `VisualNovel.Core` - Core game systems
- `VisualNovel.UI` - UI components
- `VisualNovel.Data` - Data structures
- `VisualNovel.Utils` - Utility functions

### Documentation

- Add XML comments to all public classes and methods
- Include usage examples for complex features
- Update README.md for major features
- Add entries to API.md for new APIs

## Testing

Before submitting:

1. Test in Unity Editor
2. Test all dialogue paths
3. Test save/load functionality
4. Check for console errors
5. Test on target platforms
6. Verify no breaking changes

## Areas for Contribution

### High Priority

- Custom Unity Editor for DialogueData
- Visual dialogue tree editor
- Localization system
- Save slot management UI
- Gallery/CG viewer system

### Medium Priority

- Animated character sprites
- Particle effects system
- Advanced text effects (shake, color, etc.)
- Achievement system
- Statistics tracking

### Low Priority

- Mobile platform optimizations
- Controller support
- Accessibility features
- Performance profiling tools

## Development Setup

1. Clone the repository
2. Open in Unity 2020.3 or newer
3. Install TextMeshPro via Package Manager
4. Open SampleScene to test

## Pull Request Process

1. Update documentation for new features
2. Add comments to complex code
3. Ensure no warnings or errors
4. Test thoroughly
5. Update CHANGELOG if applicable
6. Request review from maintainers

## Code Review

All submissions require review. We look for:

- Code quality and style
- Proper documentation
- No breaking changes
- Performance considerations
- Compatibility

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## Questions?

Feel free to open an issue for questions or discussions!

## Recognition

Contributors will be recognized in the project credits. Thank you for helping make this project better!
